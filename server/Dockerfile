# Use Python 3 as a base image
FROM python:3

# Set the working directory to /usr/src/app
WORKDIR /usr/src/app

# install dependencies
COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

# copy the current directory contents into the container at /usr/src/app
COPY . /usr/src/app

# TODO: add production settings for Django

# run and make migrations
RUN python manage.py makemigrations
RUN python manage.py migrate

# set debug to false
ENV DEBUG=False

# Run Gunicorn
CMD ["gunicorn", "--bind", "0.0.0.0:8443", "egglocke.wsgi:application"]

EXPOSE 8443
# debug, expose all ports


