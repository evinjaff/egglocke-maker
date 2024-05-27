from django.shortcuts import render, get_object_or_404
from django.template import loader
from django.http import Http404, HttpResponse, HttpResponseRedirect
from django.db.models import F
from django.views import generic
from django.utils import timezone

# Create your views here.
from django.urls import reverse
from .models import Question, Choice

class HomeView(generic.TemplateView):
    template_name = "pokepoll/home.html"

class IndexView(generic.ListView):
    template_name = "pokepoll/submissions.html"
    context_object_name = "latest_question_list"
    
    def get_queryset(self):
        """
        Return the last five published questions (not including those set to be
        published in the future).
        """
        return Question.objects.filter(pub_date__lte=timezone.now()).order_by("-pub_date")[
            :5
        ]


class DetailView(generic.DetailView):
    model = Question
    template_name = "pokepoll/detail.html"


class ResultsView(generic.DetailView):
    model = Question
    template_name = "pokepoll/results.html"


def vote(request, question_id):
    question = get_object_or_404(Question, pk=question_id)
    try:
        selected_choice = question.choice_set.get(pk=request.POST["choice"])
    except (KeyError, Choice.DoesNotExist):
        # Redisplay the question voting form.
        return render(
            request,
            "pokepoll/detail.html",
            {
                "question": question,
                "error_message": "You didn't select a choice.",
            },
        )
    else:
        selected_choice.votes = F("votes") + 1
        selected_choice.save()
        # Always return an HttpResponseRedirect after successfully dealing
        # with POST data. This prevents data from being posted twice if a
        # user hits the Back button.
        return HttpResponseRedirect(reverse("pokepoll:results", args=(question.id,)))