from django.urls import path
from django.urls.conf import include

from .views import auth_views

from . import unfiled_views

app_name = "pokepoll"
urlpatterns = [
    path("", unfiled_views.HomeView.as_view(), name="home"),
    path("submissions/", unfiled_views.IndexView.as_view(), name="submissions"),
    path("<int:pk>/", unfiled_views.DetailView.as_view(), name="detail"),
    path("<int:pk>/results/", unfiled_views.ResultsView.as_view(), name="results"),
    path("<int:question_id>/vote/", unfiled_views.vote, name="vote"),
    path("submit/", unfiled_views.MasterPokemonAndSubmitterView.as_view(), name="submit"),
    path("downloadSaveFile/", unfiled_views.saveGenView, name="downloadSaveFile"),
    path("submit-showdown/", unfiled_views.showdown_decoder, name="submit-showdown" ),
    # Account Related Views
    path('register/', auth_views.RegisterUserView.as_view(), name='register-user'),
    path('login/', auth_views.login_view, name='login'),
    path('logout/', auth_views.logout_view, name='logout')
]