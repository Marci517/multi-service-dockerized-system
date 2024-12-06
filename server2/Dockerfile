FROM python:3.9-slim

WORKDIR /app

COPY req.txt /app/

RUN pip install --no-cache-dir -r req.txt

COPY . /app/

EXPOSE 8080

CMD ["python", "server.py"]
