# Generated by Django 5.0.6 on 2024-05-28 01:01

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('pokepoll', '0009_alter_pokemon_pokemon_moves_and_more'),
    ]

    operations = [
        migrations.AddField(
            model_name='pokemon',
            name='pokemon_held_item',
            field=models.IntegerField(default=0),
        ),
    ]
