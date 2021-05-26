FROM python:3.9-alpine

WORKDIR /app

# install psycopg2 dependencies
RUN apk update && apk add postgresql-dev gcc python3-dev musl-dev

#copy requirements and build them
RUN pip install --upgrade pip
COPY ./requirements.txt .
RUN pip install -r requirements.txt

#copy the project code
COPY . .