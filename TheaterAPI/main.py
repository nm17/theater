import secrets

import flask
import jwt
import tinydb
import json
from passlib.hash import argon2
from tinydb.operations import set

app = flask.Flask(__name__)

db = tinydb.TinyDB('ticketsdb.json')
tickets_table = db.table('tickets')
users_table = db.table('users')
secret = 'U37aBd_uXeCXWDQs4_Qa2b7Eh3ay3Hq9NJ71Jhrjb_M'

server_config = json.load(open('server_confi.json'))

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


def get_authernticated_id(content: dict):
    try:
        return jwt.decode(content.get('jwt', ''), secret)['id']
    except jwt.DecodeError:
        return None


def check_if_username_exist(username: str):
    q = tinydb.Query()
    result = tickets_table.search(q.username == username)
    return len(result) != 0


@app.route('/theaterapi/placeinfo', methods=['POST'])
def isfree():
    content = flask.request.json
    row = content.get('row', 0)
    place = content.get('place', 0)

    if type(row) is not int and type(place) is not int:
        return flask.jsonify({'error': 403}), 403

    q = tinydb.Query()
    search_result = tickets_table.search((q.row == row) & (q.place == place))
    if len(search_result) != 1:
        return flask.jsonify({'error': 404}), 404
    return flask.jsonify(search_result[0])


@app.route('/theaterapi/register', methods=['POST'])
def register():
    content = flask.request.json
    username = content.get('username', None)
    password = content.get('password', None)
    if username is None and password is None and check_if_username_exist(username):
        return flask.jsonify({'error': 403}), 403

    users_table.insert({
        'username': username,
        'password': argon2.hash(password),
        'id': secrets.token_urlsafe(24),
        'admin': False
    })
    return ''


@app.route('/theaterapi/login', methods=['POST'])
def login():
    content = flask.request.json
    username = content.get('username', None)
    password = content.get('password', None)

    q = tinydb.Query()
    db.contains(q.username == username)
    result = users_table.search(q.username == username)[0]
    admin = result['admin']
    if argon2.verify(password, result['password']):
        return flask.jsonify({'jwt': jwt.encode({
            'id': result["id"],
            'admin': admin
        }, secret).decode()})


@app.route('/theaterapi/book', methods=['POST'])
def book():
    content = flask.request.json
    row = content.get('row', 0)
    place = content.get('place', 0)

    if type(row) is not int and type(place) is not int:
        return flask.jsonify({'error': 403}), 403
    id_ = get_authernticated_id(content)

    q = tinydb.Query()
    booked_already = len(tickets_table.search((q.free == True) & (q.row == row) & (q.place == place))) == 0

    if id_ is None or booked_already:
        return flask.jsonify({'error': 401}), 401

    tickets_table.update(set('id', id_),
                         (q.row == row) & (q.place == place)
                         )
    tickets_table.update(set('free', False),
                         (q.row == row) & (q.place == place)
                         )
    return ''


app.run(host='192.168.1.72', port=80)
