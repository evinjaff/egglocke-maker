docker build -t server . 

docker run -d -it -p 8443:8443 server
