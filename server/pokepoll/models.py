import datetime
# Create your models here.
from django.db import models
from django.utils import timezone

# User class - since we don't need login functionality, 
# we just want to know who submitted the form
class Submitter(models.Model):
    name = models.CharField(max_length=200)
    # email = models.EmailField(max_length=254, unique=True)
    tripcode = models.TextField(max_length=128)

    @classmethod
    def get_default_parent_key(cls):
        submitter, created = cls.objects.get_or_create(
            name="Anonymous", tripcode="0" * 64
        )

        return submitter.pk

    def __str__(self):
        return self.name + " (" + self.tripcode + ")"
    
class Pokemon(models.Model):
    pokemon_nickname = models.CharField(max_length=10, default="")
    pokemon_species = models.IntegerField(default=1)
    pokemon_ball = models.IntegerField(default=1)
    pokemon_language = models.IntegerField(default=2)
    pokemon_ability = models.IntegerField(default=1)
    pokemon_nature = models.IntegerField(default=1)
    pokemon_OT = models.CharField(max_length=10, default="Red")
    pokemon_OTGender = models.IntegerField(default=1)
    pokemon_IV = models.JSONField(default=list([31, 31, 31, 31, 31, 31]))
    pokemon_EV = models.JSONField(default=list([0, 0, 0, 0, 0, 0]))
    pokemon_moves = models.JSONField(default=list([1]))
    pokemon_movespp = models.JSONField(default=list([0]))
    pokemon_held_item = models.IntegerField(default=0)
    pokemon_is_shiny = models.BooleanField(default=False)
    pokemon_intended_generation = models.IntegerField(default=4, choices=[(1, "Gen 1"), (2, "Gen 2"), (3, "Gen 3"), (4, "Gen 4"), (5, "Gen 5"), (6, "Gen 6"), (7, "Gen 7"), (8, "Gen 8")])


    pub_date = models.DateTimeField("date published")
    submitter = models.ForeignKey(Submitter, on_delete=models.CASCADE, default=Submitter.get_default_parent_key)
    upvotes = models.IntegerField(default=0)

    def was_published_recently(self):
        now = timezone.now()
        return now - datetime.timedelta(days=1) <= self.pub_date <= now

    def __str__(self):
        return "{} ({})\nAbility: {}".format(self.pokemon_nickname, self.pokemon_species, self.pokemon_ability)