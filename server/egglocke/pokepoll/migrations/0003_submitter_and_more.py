# Generated by Django 5.0.6 on 2024-05-27 04:47

import django.db.models.deletion
import pokepoll.models
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('pokepoll', '0002_rename_question_pokemon'),
    ]

    operations = [
        migrations.CreateModel(
            name='Submitter',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('name', models.CharField(max_length=200)),
                ('email', models.EmailField(max_length=254)),
            ],
        ),
        migrations.RenameField(
            model_name='pokemon',
            old_name='question_text',
            new_name='pokemon_nickname',
        ),
        migrations.AddField(
            model_name='pokemon',
            name='pokemon_species',
            field=models.IntegerField(default=1),
        ),
        migrations.AddField(
            model_name='pokemon',
            name='submitter',
            field=models.ForeignKey(default=pokepoll.models.Submitter.get_default_parent_key, on_delete=django.db.models.deletion.CASCADE, to='pokepoll.submitter'),
        ),
        migrations.DeleteModel(
            name='Choice',
        ),
    ]
