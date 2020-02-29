import anachronos

from test.runner import http


class ExceptionMapperTests(anachronos.TestCase):
    def test_get(self):
        res = http.get("/")

        self.assertEqual(404, res.status_code)


if __name__ == '__main__':
    anachronos.run_tests()
