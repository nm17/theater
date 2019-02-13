import datetime
import secrets

import flask
import jwt
import tinydb
import json
from passlib.hash import argon2
from tinydb.operations import set

app = flask.Flask('TheaterAPI')

db = tinydb.TinyDB('ticketsdb.json')
tickets_table = db.table('tickets')
users_table = db.table('users')
secret = 'U37aBd_uXeCXWDQs4_Qa2b7Eh3ay3Hq9NJ71Jhrjb_M'

server_config = json.load(open('server_config.json'))


def add_admins():
    """Добавляем админов из server_config.json"""
    for admin_creds in server_config['admins']:
        q = tinydb.Query()
        if len(users_table.search(q.username == admin_creds['username'])) != 0:
            continue
        users_table.insert({
            'username': admin_creds['username'],
            'password': admin_creds['password'],
            'id': secrets.token_urlsafe(24),
            'admin': True
        })


def check_if_admin(f):
    def wrapper():
        try:
            if isinstance(flask.request.json, dict) is False:
                raise AttributeError
        except AttributeError:
            return flask.jsonify({'error': 400}), 400
        content = flask.request.json
        jwt_token = content.get('jwt', '')
        try:
            jwt_token = jwt.decode(jwt_token, secret, algorithms=['HS256'])
        except jwt.DecodeError:
            return flask.jsonify({'error': 401}), 401
        user_id = jwt_token['id']
        q = tinydb.Query()
        results = users_table.search((q.id == user_id) & (q.admin == True))
        if len(results) == 0:
            flask.jsonify({'error': 401}), 401
        f()
    return wrapper


def check_if_place_exist(row: int, place: int):
    q = tinydb.Query()
    results = tickets_table.search((q.row == row) & (q.place == place))
    return len(results) != 0


def get_authernticated_id(content: dict):
    """
    Возвращает токен пользователя если токен подтверженный,
    иначе возвращает None
    :param content: JSON запроса
    :return: Токен или None
    """
    try:
        return jwt.decode(content.get('jwt', ''), secret, algorithms=['HS256'])['id']
    except jwt.DecodeError:
        return None


def check_if_username_exist(username: str):
    """Проверяет существование пользователя"""
    q = tinydb.Query()
    result = users_table.search(q.username == username)
    return len(result) != 0


@app.route('/theaterapi/scheme', methods=['GET'])
def scheme():
    return flask.jsonify(server_config['scheme'])


@app.route('/theaterapi/placeinfo', methods=['POST'])
def placeinfo():
    try:
        if isinstance(flask.request.json, dict) is False:
            raise AttributeError
    except AttributeError:
        return flask.jsonify({'error': 400}), 400
    content = flask.request.json
    row = content.get('row', '')
    place = content.get('place', '')

    if not isinstance(row, int) or not isinstance(place, int):
        return flask.jsonify({'error': 400}), 400

    q = tinydb.Query()
    search_result = tickets_table.search((q.row == row) & (q.place == place))
    print(search_result)
    print('oooooo')

    if len(search_result) != 1:
        return flask.jsonify({'error': 404}), 404
    return flask.jsonify(search_result[0])


@app.route('/theaterapi/register', methods=['POST'])
def register():
    try:
        if isinstance(flask.request.json, dict) is False:
            raise AttributeError
    except AttributeError:
        return flask.jsonify({'error': 400}), 400
    content = flask.request.json
    username = content.get('username', None)
    password = content.get('password', None)
    if type(username) is not str or type(password) is not str or check_if_username_exist(username):
        return flask.jsonify({'error': 400}), 400

    users_table.insert({
        'username': username,
        'password': argon2.hash(password),
        'id': secrets.token_urlsafe(24),
        'admin': False
    })
    return ''


@app.route('/theaterapi/login', methods=['POST'])
def login():
    try:
        if isinstance(flask.request.json, dict) is False:
            raise AttributeError
    except AttributeError:
        return flask.jsonify({'error': 400}), 400
    content = flask.request.json
    username = content.get('username', None)
    password = content.get('password', None)

    q = tinydb.Query()
    db.contains(q.username == username)
    result = users_table.search(q.username == username)

    if len(result) != 1:
        return flask.jsonify({'error': 400}), 400

    result = result[0]
    admin = result['admin']
    if argon2.verify(password, result['password']):
        jwt_token = jwt.encode({
            'id': result["id"]
        }, secret).decode()
        return flask.jsonify({'jwt': jwt_token, 'admin': admin})
    return flask.jsonify({'error': 400}), 400


@app.route('/theaterapi/book', methods=['POST'])
def book():
    try:
        if isinstance(flask.request.json, dict) is False:
            raise AttributeError
    except AttributeError:
        return flask.jsonify({'error': 400}), 400
    content = flask.request.json
    row = content.get('row', None)
    place = content.get('place', None)

    if isinstance(row, int) is False or isinstance(place, int) is False:
        return flask.jsonify({'error': 400}), 400
    id_ = get_authernticated_id(content)

    q = tinydb.Query()
    booked_already = len(tickets_table.search((q.free == False) & (q.row == row) & (q.place == place))) == 1
    print(booked_already)

    if id_ is None:
        return flask.jsonify({'error': 401}), 401

    if booked_already:
        return flask.jsonify({'error': 403}), 403

    tickets_table.update(set('id', id_),
                         (q.row == row) & (q.place == place)
                         )
    tickets_table.update(set('free', False),
                         (q.row == row) & (q.place == place)
                         )
    return ''


@check_if_admin
@app.route('/theaterapi/addticket', methods=['POST'])
def addticket():
    try:
        if isinstance(flask.request.json, dict) is False:
            raise AttributeError
    except AttributeError:
        return flask.jsonify({'error': 400}), 400
    content = flask.request.json
    row = content.get('row', None)
    place = content.get('place', None)
    price = content.get('price', None)

    if isinstance(row, int) is False or isinstance(place, int) is False or \
            isinstance(price, int) is False:
        return flask.jsonify({'error': 400}), 400

    if check_if_place_exist(row, place):
        return flask.jsonify({'error': 400}), 400

    tickets_table.insert({
        'row': row,
        'place': place,
        'price': price,
        'free': True,
        'id': ''
    })
    return ''


@app.route('/theaterapi/setscheme')


if __name__ == '__main__':
    add_admins()
    app.run(host='192.168.1.72', port=80)
