import anachronos

from test.runner import http


class ExceptionMapperTests(anachronos.TestCase):

    def test_basic_exception_mapping(self):
        res = http.get("/exception")

        self.assertEqual(200, res.status_code)
        self.assertEqual("got a simple exception!", res.json()["Message"])


if __name__ == '__main__':
    anachronos.run_tests()
