#!/bin/bash

list_of_envs="dev,prod"
container_or_image_name="api"

DisplayHelp()
{
   echo "runs server with specified environment."
   echo "options:"
   echo "-e|--env     Environment ($list_of_envs)"
   echo "-h|--help    Help"
}

while [ "$#" -gt 0 ]; do
    case $1 in
        -e|--env) env="$2"; shift ;;
        -h|--help) DisplayHelp ; exit 0 ;;
        *) echo "Unknown parameter passed: $1"; exit 1 ;;
    esac
    shift
done


# list_include_item "dev,prod" "dev"
function list_include_item {
  local list="$1"
  local item="$2"
  if [[ $list =~ (^|[","])"$item"($|[","]) ]] ; then
    result=0
  else
    result=1
  fi
  return $result
}

env=$([ "$env" == "" ] && echo "dev" || echo $env)

ASPNETCORE_ENVIRONMENT=$([ "$env" == "dev" ] && echo "Development" || echo "Production")

if `list_include_item "$list_of_envs" $env` ; then
  docker build -t $container_or_image_name:$env .
  container_exists=$(docker container ls -a -q -f name=^/${container_or_image_name}$)
  if [ "${container_exists}" ]; then
    container_is_running=$(docker container ls -a -q -f status=running -f name=^/${container_or_image_name}$)
    if [ "${container_is_running}" ]; then
        docker container stop $container_or_image_name
    fi
    docker container rm $container_or_image_name
    unset container_is_running
  fi
  unset container_exists
  docker run --name $container_or_image_name -p 8080:80 -e ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT $container_or_image_name:$env
else 
  echo "env must be either dev or prod"
fi

unset container_or_image_name
unset list_of_envs
