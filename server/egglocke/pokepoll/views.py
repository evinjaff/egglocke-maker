import os
from django.shortcuts import render, get_object_or_404
from django.template import loader
from django.http import Http404, HttpResponse, HttpResponseRedirect, JsonResponse
from django.db.models import F
from django.views import generic
from django.utils import timezone
from django import forms
from django.forms import TextInput, EmailInput
import json
# import variables in settings.py
from django.conf import settings
from .cachedconstants import MAX_POKEDEX_DICT
from django.views.decorators.http import require_GET, require_POST
from django.core.cache import cache
from django_ratelimit.decorators import ratelimit
from django.utils.decorators import method_decorator

# Create your views here.
from django.urls import reverse
from .models import Pokemon, Submitter

@require_GET
def robots_txt_view(request):
    robots_txt_path = "pokepoll/support/robots.txt"
    content_type = "text/plain"

    robots_txt_content = cache.get("robots_txt_content")

    if robots_txt_content is None:
        with open(os.path.join(settings.BASE_DIR, robots_txt_path)) as f:
            robots_txt_content = f.read()
            cache.set("robots_txt_content", robots_txt_content, timeout=60 * 60 * 12)

    return HttpResponse(robots_txt_content, content_type=content_type)


class HomeView(generic.TemplateView):
    template_name = "pokepoll/home.html"

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context["num_eggs_submitted"] = self.get_num_eggs_submitted()

        if Pokemon.objects.count() > 0:
            context["most_recent_pokemon"] = Pokemon.objects.order_by("-pub_date")[0]
        else:
            context["most_recent_pokemon"] = None


        return context

    def get_num_eggs_submitted(self):
        return Pokemon.objects.count()

class IndexView(generic.ListView):
    template_name = "pokepoll/submissions.html"
    context_object_name = "latest_pokemon_list"
    
    def get_queryset(self):
        """
        Return the last five published questions (not including those set to be
        published in the future).
        """
        return Pokemon.objects.filter(pub_date__lte=timezone.now()).order_by("-pub_date")[
            :50
        ]


class DetailView(generic.DetailView):
    model = Pokemon
    template_name = "pokepoll/detail.html"
    context_object_name = "pokemon"


class ResultsView(generic.DetailView):
    model = Pokemon
    template_name = "pokepoll/results.html"


class PokemonForm(forms.ModelForm):
    class Meta:
        model = Pokemon
        fields = ['pokemon_nickname', 'pokemon_species', 'pokemon_ball', 'pokemon_language', 'pokemon_ability', 'pokemon_nature', 'pokemon_OT', 'pokemon_OTGender', 'pokemon_IV', 'pokemon_EV', 'pokemon_moves', 'pokemon_movespp']

class SubmitterForm(forms.ModelForm):
    class Meta:
        model = Submitter
        fields = ['email', 'name']
        widgets = {
            'name': TextInput(attrs={
                'class': "form-control",
                'style': 'max-width: 300px;',
                'placeholder': 'Name'
                }),
            'email': EmailInput(attrs={
                'class': "form-control", 
                'style': 'max-width: 300px;',
                'placeholder': 'Email'
                })
        }

class SaveGenView(generic.TemplateView):
    # WIP view
    template_name = "pokepoll/save_gen.html"

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        return context

    def post(self, request, *args, **kwargs):
        return HttpResponseRedirect(reverse('pokepoll:home'))

@method_decorator(ratelimit(key='ip', rate='3/m', method='POST', block=True), name='dispatch')
@method_decorator(ratelimit(key='ip', rate='20/m', method='GET', block=True), name='dispatch')
class MasterPokemonAndSubmitterView(generic.TemplateView):
    template_name = 'pokepoll/master_submit.html'

    def get(self, request, *args, **kwargs):
        submitter_form = SubmitterForm()
        song_form = PokemonForm()
        return render(request, self.template_name, {
            'submitter_form': submitter_form,
            'pokemon_form': song_form,
            'max_pokedex_entry': MAX_POKEDEX_DICT[settings.POKEMON_GENERATION],
        })

    def post(self, request, *args, **kwargs):

        print(request.POST)

        submitter_form = SubmitterForm(request.POST)
        song_form = SubmitterForm(request.POST)

        # add validation here

        # if email is in database
        foreign_key = None
        pokemon_species = None
        IVs = [31, 31, 31, 31, 31, 31]
        EVs = [0, 0, 0, 0, 0, 0]

        if Submitter.objects.filter(email=request.POST.get('email')).exists():
            print("email: {} | ".format(request.POST.get('email')))
            # since emails are unique, we can get the first one
            emails = Submitter.objects.get(email=request.POST.get('email'))
            foreign_key = emails.id

        else:
            # create a new submitter
            submitter = Submitter(
                name=request.POST.get('name'),
                email=request.POST.get('email')
            )

            submitter.save()
            foreign_key = submitter.id

        # translate species to pokedex number
        with open(os.path.join(settings.BASE_DIR, 'pokepoll/static/pokepoll/pokemon_name_to_id.json')) as f:
            pokedex = json.load(f)
            if request.POST.get('pokemon_species') in pokedex:
                pokemon_species = pokedex[request.POST.get('pokemon_species')]
            else:
                pokemon_species = None

        #IVs
        # populate IVs with the values from the form
        for i in range(6):
            IVs[i] = int(request.POST.get('IV' + str(i + 1)))

        #EVs
        # populate EVs with the values from the form
        for i in range(6):
            EVs[i] = int(request.POST.get('EV' + str(i + 1)))

        # translate moves and movespp to int format

        # create a new pokemon
        pokemon = Pokemon(
            pokemon_nickname=request.POST.get('pokemon_nickname'),
            pokemon_species=pokemon_species,
            pub_date=timezone.now(),
            submitter_id=foreign_key,
            pokemon_IV=IVs,
            pokemon_EV=EVs

        )
        pokemon.save()

        return HttpResponseRedirect(reverse('pokepoll:home'))




        # if submitter_form.is_valid() and song_form.is_valid():
        #     submitter = submitter_form.save()
        #     song = song_form.save(commit=False)
        #     song.submitter = submitter
        #     song.save()

        #     return HttpResponseRedirect(reverse('pokepoll:home'))
        
        # return render(request, self.template_name, {
        #     'submitter_form': submitter_form,
        #     'pokemon_form': song_form
        # })


def vote(request, question_id):
    question = get_object_or_404(Pokemon, pk=question_id)
    try:
        selected_choice = question.choice_set.get(pk=request.POST["choice"])
    except (KeyError, Submitter.DoesNotExist):
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
    
def validate_submitter(request):
    email = request.POST.get('email', None)
    data = {
        'is_taken': Submitter.objects.filter(email__iexact=email).exists()
    }
    return JsonResponse(data)