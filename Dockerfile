FROM python:3.9-alpine

WORKDIR /app

#copy requirements and build them
RUN pip install --upgrade pip
COPY ./requirements.txt .
RUN pip install -r requirements.txt

#copy the project code
COPY . .