# Generated by Django 5.0.6 on 2024-05-27 09:38

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('pokepoll', '0005_pokemon_pokemon_ev_pokemon_pokemon_iv_and_more'),
    ]

    operations = [
        migrations.AlterField(
            model_name='pokemon',
            name='pokemon_EV',
            field=models.JSONField(default=[0, 0, 0, 0, 0, 0]),
        ),
        migrations.AlterField(
            model_name='pokemon',
            name='pokemon_IV',
            field=models.JSONField(default=[31, 31, 31, 31, 31, 31]),
        ),
    ]
