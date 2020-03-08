#!/bin/sh
export PYTHONPATH=$(pwd)/e2e_tests:$PYTHONPATH
python e2e_tests/test/runner.py
