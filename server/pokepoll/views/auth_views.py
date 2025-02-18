from django.shortcuts import render, redirect
from django.contrib.auth import authenticate, login
from django.contrib.auth.forms import AuthenticationForm

from django.contrib.auth.models import User
from rest_framework import status
from rest_framework.response import Response
from rest_framework.views import APIView
from django.db import transaction
from django.contrib.auth import logout

from ..models import Submitter

def login_view(request):
    if request.method == 'POST':
        form = AuthenticationForm(request, data=request.POST)
        if form.is_valid():
            username = form.cleaned_data.get('username')
            password = form.cleaned_data.get('password')
            user = authenticate(username=username, password=password)
            if user is not None:
                login(request, user)
                # Redirect to your desired page after successful login
                return redirect("/pokepoll")  # Replace 'home' with your home page URL name
            else:
                # Handle invalid login (e.g., display an error message)
                form.add_error(None, "Invalid username or password.") # adds a non-field error
    else:
        form = AuthenticationForm()
    return render(request, 'pokepoll/login.html', {'form': form})

def logout_view(request):
    logout(request)
    # Redirect to a success page.

    return redirect("/pokepoll")




class RegisterUserView(APIView):
	def get(self, request):
		return render(request, 'pokepoll/registration.html')  # Render the registration form

	def post(self, request):
		username = request.data.get("username")
		email = request.data.get("email")
		password = request.data.get("password")
		name = request.data.get("name", username)  # Default to username if no name is provided

		if not username or not email or not password:
			return Response({"error": "Username, email, and password are required."}, status=status.HTTP_400_BAD_REQUEST)

		if User.objects.filter(username=username).exists():
			return Response({"error": "Username already taken."}, status=status.HTTP_400_BAD_REQUEST)

		if User.objects.filter(email=email).exists():
			return Response({"error": "Email already registered."}, status=status.HTTP_400_BAD_REQUEST)

		with transaction.atomic():
			user = User.objects.create_user(username=username, email=email, password=password)
			submitter = Submitter.objects.create(user=user, name=name)

		return Response({"message": "User registered successfully", "submitter_id": submitter.id}, status=status.HTTP_201_CREATED)
