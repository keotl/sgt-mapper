import subprocess
import time

import anachronos
from anachronos.configuration import ApplicationRunner, DefaultRunner
from anachronos.util.http_requester import HttpRequester
from jivago.lang.annotations import Override

from test import tests
import app
import os.path

@DefaultRunner
class AppRunner(ApplicationRunner):

    @Override
    def run(self):
        appDir = os.path.dirname(app.__file__)
        subprocess.call(["dotnet", "build", f"{appDir}/app.csproj", "-c", "Release"])
        self.process = subprocess.Popen([f"{appDir}/bin/Release/netcoreapp3.1/app"])
        time.sleep(5)

    @Override
    def stop(self):
        self.process.terminate()

    @Override
    def app_run_function(self) -> None:
        # unused
        pass


http = HttpRequester("http://localhost", port=5000)

if __name__ == '__main__':
    anachronos.discover_tests(tests)
    anachronos.run_tests()
