from django.urls import path
from django.urls.conf import include

from . import views

app_name = "pokepoll"
urlpatterns = [
    path("", views.HomeView.as_view(), name="home"),
    path("submissions/", views.IndexView.as_view(), name="submissions"),
    path("<int:pk>/", views.DetailView.as_view(), name="detail"),
    path("<int:pk>/results/", views.ResultsView.as_view(), name="results"),
    path("<int:question_id>/vote/", views.vote, name="vote"),
    path("submit/", views.MasterPokemonAndSubmitterView.as_view(), name="submit"),
    path("validateSubmitter/", views.validate_submitter, name="validateSubmitter"),
    path("downloadSaveFile/", views.saveGenView, name="downloadSaveFile"),
    
    

]