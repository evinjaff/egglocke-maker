docker build -t server-dev -f Dockerfile-dev .

docker run -it -p 8443:8443 -t server-dev
