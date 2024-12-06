import os
from flask import Flask, jsonify
import psycopg2

# Flask inicializálása
app = Flask(__name__)

# PostgreSQL kapcsolat beállítása
def get_db_connection():
    conn = psycopg2.connect(
        dbname=os.getenv("DB_NAME"),
        user=os.getenv("DB_USER"),
        password=os.getenv("DB_PASSWORD"),
        host=os.getenv("DB_HOST", "db"),
        port="5432"
    )
    return conn

# Egy entitás lekérése az id alapján
@app.route('/menu/<int:id>', methods=['GET'])
def get_entity(id):
    conn = get_db_connection()
    cursor = conn.cursor()
    cursor.execute('SELECT * FROM MENU WHERE id = %s', (id,))
    entity = cursor.fetchone()
    cursor.close()
    conn.close()

    if entity:
        return jsonify({'id': entity[0], 'name': entity[1], 'restaurant_id': entity[2]}), 200
    else:
        return jsonify({'error': 'Entity not found'}), 404

# Az összes entitás lekérése
@app.route('/menus', methods=['GET'])
def get_all_entities():
    conn = get_db_connection()
    cursor = conn.cursor()
    cursor.execute('SELECT * FROM MENU')
    entities = cursor.fetchall()
    cursor.close()
    conn.close()

    if entities:
        result = [{'id': entity[0], 'name': entity[1], 'restaurant_id': entity[2]} for entity in entities]
        return jsonify(result), 200
    else:
        return jsonify({'error': 'No entities found'}), 404

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=8080)
