# Find and kill any Docker containers bound to port 8443
$containerId = docker ps --filter "publish=8443" --format "{{.ID}}"
if ($containerId) {
    docker rm -f $containerId
}

docker build -t server_test_prod -f Dockerfile .

docker run -it -p 8443:8443 server_test_prod
