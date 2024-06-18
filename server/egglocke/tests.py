from django.test import TestCase
from django.urls import reverse

from http import HTTPStatus


class RobotTxtTests(TestCase):
    def test_robots_txt_view(self):
        response = self.client.get("/robots.txt")
        
        assert response.status_code == HTTPStatus.OK
        assert response["content-type"] == "text/plain"

        valid_user_agent_headers = [ "User-Agent", "user-agent", "USER-AGENT", "User-agent" ]
        valid_header_seen = False

        for header in valid_user_agent_headers:
            if header in response.content.decode():
                valid_header_seen = True
                break

        assert valid_header_seen

    def test_cannot_access_robots_txt_view_with_post(self):
        response = self.client.post("/robots.txt")
        self.assertEqual(response.status_code, 405)