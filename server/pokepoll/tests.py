# Create your tests here.
from contextlib import AbstractContextManager
import datetime
from typing import Any

from django.test import TestCase
from django.utils import timezone
from django.urls import reverse
from .models import Pokemon, Submitter

class QuestionModelTests(TestCase):
    def test_was_published_recently_with_future_question(self):
        """
        was_published_recently() returns False for questions whose pub_date
        is in the future.
        """
        time = timezone.now() + datetime.timedelta(days=30)
        future_question = Pokemon(pub_date=time)
        self.assertIs(future_question.was_published_recently(), False)

    def test_was_published_recently_with_old_question(self):
        """
        was_published_recently() returns False for questions whose pub_date
        is older than 1 day.
        """
        time = timezone.now() - datetime.timedelta(days=1, seconds=1)
        old_question = Pokemon(pub_date=time)
        self.assertIs(old_question.was_published_recently(), False)


    def test_was_published_recently_with_recent_question(self):
        """
        was_published_recently() returns True for questions whose pub_date
        is within the last day.
        """
        time = timezone.now() - datetime.timedelta(hours=23, minutes=59, seconds=59)
        recent_question = Pokemon(pub_date=time)
        self.assertIs(recent_question.was_published_recently(), True)

def create_question(question_text, days):
    """
    Create a question with the given `question_text` and published the
    given number of `days` offset to now (negative for questions published
    in the past, positive for questions that have yet to be published).
    """
    time = timezone.now() + datetime.timedelta(days=days)
    return Pokemon.objects.create(question_text=question_text, pub_date=time)


class QuestionIndexViewTests(TestCase):
    def test_no_questions(self):
        """
        If no questions exist, an appropriate message is displayed.
        """
        response = self.client.get(reverse("pokepoll:index"))
        self.assertEqual(response.status_code, 200)
        self.assertContains(response, "No polls are available.")
        self.assertQuerySetEqual(response.context["latest_question_list"], [])

    def test_past_question(self):
        """
        Questions with a pub_date in the past are displayed on the
        index page.
        """
        question = create_question(question_text="Past question.", days=-30)
        response = self.client.get(reverse("pokepoll:index"))
        self.assertQuerySetEqual(
            response.context["latest_question_list"],
            [question],
        )

    def test_future_question(self):
        """
        Questions with a pub_date in the future aren't displayed on
        the index page.
        """
        create_question(question_text="Future question.", days=30)
        response = self.client.get(reverse("pokepoll:index"))
        self.assertContains(response, "No polls are available.")
        self.assertQuerySetEqual(response.context["latest_question_list"], [])

    def test_future_question_and_past_question(self):
        """
        Even if both past and future questions exist, only past questions
        are displayed.
        """
        question = create_question(question_text="Past question.", days=-30)
        create_question(question_text="Future question.", days=30)
        response = self.client.get(reverse("pokepoll:index"))
        self.assertQuerySetEqual(
            response.context["latest_question_list"],
            [question],
        )

    def test_two_past_questions(self):
        """
        The questions index page may display multiple questions.
        """
        question1 = create_question(question_text="Past question 1.", days=-30)
        question2 = create_question(question_text="Past question 2.", days=-5)
        response = self.client.get(reverse("pokepoll:index"))
        self.assertQuerySetEqual(
            response.context["latest_question_list"],
            [question2, question1],
        )

import random

def create_random_pokemon(override_dict=None):
    """
    Create a question with a sample pokemon
    """
    time = timezone.now()

    # if an override dictionary is provided, use that to create the pokemon
    if override_dict:
        return Pokemon.objects.create(pokemon_nickname=override_dict.get("pokemon_nickname", "Sample"), 
                                  pub_date=time,
                                  pokemon_species=override_dict.get("pokemon_species", random.randint(1,151)),
                                  pokemon_ball=override_dict.get("pokemon_ball", random.randint(1,5)),
                                    pokemon_language=override_dict.get("pokemon_language", random.randint(1,3)),
                                    pokemon_ability=override_dict.get("pokemon_ability", random.randint(1,4)),
                                    pokemon_nature=override_dict.get("pokemon_nature", random.randint(1,25)),
                                    pokemon_OT=override_dict.get("pokemon_OT", "Red"),
                                    pokemon_OTGender=override_dict.get("pokemon_OTGender", 1),
                                    pokemon_IV=override_dict.get("pokemon_IV", [random.randint(0,31) for i in range(6)]),
                                    pokemon_EV=override_dict.get("pokemon_EV", [random.randint(0,252) for i in range(6)]),
                                    pokemon_moves=override_dict.get("pokemon_moves", [random.randint(1,826) for i in range(random.randint(1,4))]),
                                    pokemon_movespp=override_dict.get("pokemon_movespp", [random.randint(1,40) for i in range(random.randint(1,4))]),
                                    pokemon_held_item=override_dict.get("pokemon_held_item", random.randint(1,5)),
                                    submitter=Submitter.objects.get(pk=1),
                                    upvotes=0)
    else:
        return Pokemon.objects.create(pokemon_nickname="Sample", 
                                  pub_date=time,
                                  pokemon_species=random.randint(1,151),
                                  pokemon_ball=random.randint(1,5),
                                    pokemon_language=random.randint(1,3),
                                    pokemon_ability=random.randint(1,4),
                                    pokemon_nature=random.randint(1,25),
                                    pokemon_OT="Red",
                                    pokemon_OTGender=1,
                                    pokemon_IV=[random.randint(0,31) for i in range(6)],
                                    pokemon_EV=[random.randint(0,252) for i in range(6)],
                                    pokemon_moves=[random.randint(1,826) for i in range(random.randint(1,4))],
                                    pokemon_movespp=[random.randint(1,40) for i in range(random.randint(1,4))],
                                    pokemon_held_item=random.randint(1,5),
                                    submitter=Submitter.objects.get(pk=1),
                                    upvotes=0)



class HomeViewTest(TestCase):

    def test_home_view_uses_correct_html_file(self):
        # add a sample pokemon
        create_random_pokemon()

        response = self.client.get(reverse("pokepoll:home"))
        self.assertEqual(response.status_code, 200)
        self.assertTemplateUsed(response, "pokepoll/home.html")

    def test_empty_home_does_not_crash(self):
        response = self.client.get(reverse("pokepoll:home"))
        self.assertEqual(response.status_code, 200)
        self.assertTemplateUsed(response, "pokepoll/home.html")

    def test_home_view_with_pokemon_does_not_crash(self):
        create_random_pokemon()
        response = self.client.get(reverse("pokepoll:home"))
        self.assertEqual(response.status_code, 200)
        self.assertTemplateUsed(response, "pokepoll/home.html")

    def test_home_view_shows_most_recent_pokemon(self):
        create_random_pokemon()
        create_random_pokemon(override_dict={"pokemon_nickname": "Sample2"})

        response = self.client.get(reverse("pokepoll:home"))
        self.assertEqual(response.status_code, 200)
        self.assertTemplateUsed(response, "pokepoll/home.html")
        self.assertContains(response, "Sample2")
