from django.contrib import admin

# Register your models here.
from .models import Pokemon, Submitter

admin.site.register(Pokemon)
admin.site.register(Submitter)