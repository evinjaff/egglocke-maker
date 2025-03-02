"""gunicorn WSGI server configuration."""
from multiprocessing import cpu_count
from os import environ


def max_workers():    
    return cpu_count()


bind = '0.0.0.0:' + environ.get('PORT', '8443')
max_requests = 1000
worker_class = 'gevent'
workers = max_workers()

# Enable access logging
accesslog = '-'  # Logs to stdout
errorlog = '-'   # Logs errors to stdout as well (optional)
loglevel = 'info'  # Sets logging level