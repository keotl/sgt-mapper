import anachronos

from test.runner import http


class PingTest(anachronos.TestCase):

    def test_ping(self):
        res = http.get("/ping")

        self.assertEqual(200, res.status_code)
        self.assertEqual("Pong!", res.text)
