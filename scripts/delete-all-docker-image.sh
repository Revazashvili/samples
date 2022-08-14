#!/bin/bash

docker rmi $(docker image ls -q)