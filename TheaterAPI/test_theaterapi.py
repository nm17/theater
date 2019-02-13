import os
import unittest

import flask
import jwt
import tinydb
from passlib.hash import argon2

import theaterapi


class TheaterAPITest(unittest.TestCase):
    # Placeinfo тесты

    def test_placeinfo(self):
        theaterapi.tickets_table = tinydb.TinyDB('test_1.json')
        client = theaterapi.app.test_client()
        self.assertEqual(client.post('/theaterapi/placeinfo').json['error'], 400)
        self.assertEqual(client.post('/theaterapi/placeinfo', json={
            'row': [],
            'place': {}
        }).json['error'], 400)
        theaterapi.tickets_table.insert({'row': 3, 'place': 4, 'free': True, 'id': ''})
        resp = client.post('/theaterapi/placeinfo', json={
            'row': 3,
            'place': 4
        })
        self.assertEqual(resp.status_code, 200)
        self.assertEqual(resp.json['row'], 3)
        self.assertEqual(resp.json['place'], 4)
        self.assertEqual(resp.json['free'], True)
        self.assertEqual(resp.json['id'], '')
        theaterapi.tickets_table.close()
        os.remove('test_1.json')

    # Register тесты

    def test_register(self):
        theaterapi.users_table = tinydb.TinyDB('test_6.json')
        client = theaterapi.app.test_client()
        self.assertEqual(client.post('/theaterapi/register', json={
            'username': [],
            'password': {}
        }).json['error'], 400)
        self.assertEqual(client.post('/theaterapi/register').json['error'], 400)
        resp = client.post('/theaterapi/register', json={
            'username': 'test_user',
            'password': 'test_password'
        })
        q = tinydb.Query()
        self.assertEqual(resp.status_code, 200)
        self.assertEqual(1, len(theaterapi.users_table.search(
            q.username == 'test_user'
        )))
        theaterapi.users_table.close()
        os.remove('test_6.json')

    # Login тесты

    def test_login(self):
        theaterapi.users_table = tinydb.TinyDB('test_4.json')
        client = theaterapi.app.test_client()
        self.assertEqual(client.post('/theaterapi/login').json['error'], 400)
        self.assertEqual(client.post('/theaterapi/login', json={
            'username': [],
            'password': {}
        }).json['error'], 400)
        self.assertEqual(client.post('/theaterapi/login', json={
            'username': '',
            'password': ''
        }).json['error'], 400)
        theaterapi.users_table.insert({
            'username': 'test_user',
            'password': argon2.hash('test_password'),
            'id': '123',
            'admin': True
        })
        resp = client.post('/theaterapi/login', json={
            'username': 'test_user',
            'password': 'test_password'
        })
        self.assertEqual(resp.status_code, 200)
        jwt.decode(resp.json['jwt'], theaterapi.secret)
        theaterapi.users_table.close()
        os.remove('test_4.json')

    def test_check_if_place_exist(self):
        theaterapi.tickets_table = tinydb.TinyDB('test.json')
        theaterapi.tickets_table.insert({
            'row': 4,
            'place': 2
        })
        self.assertTrue(theaterapi.check_if_place_exist(4, 2))
        theaterapi.tickets_table.close()
        os.remove('test.json')

    def test_authenticated_id(self):
        self.assertEqual(None, theaterapi.get_authernticated_id({}))

    def test_book(self):
        theaterapi.tickets_table = tinydb.TinyDB('test_11.json')
        theaterapi.users_table = tinydb.TinyDB('test_12.json')
        client = theaterapi.app.test_client()
        self.assertEqual(client.post('/theaterapi/book').json['error'], 400)
        self.assertEqual(client.post('/theaterapi/book', json={}).json['error'], 400)
        self.assertEqual(client.post('/theaterapi/book', json={
            'jwt': {},
            'row': [],
            'place': []
        }).json['error'], 400)
        self.assertEqual(client.post('/theaterapi/book', json={
            'jwt': '',
            'row': 1,
            'place': 1
        }).json['error'], 401)
        self.assertEqual(client.post('/theaterapi/book', json={
            'jwt': jwt.encode({'id': ''}, theaterapi.secret).decode(),
            'row': 1,
            'place': 1
        }).status_code, 200)
        theaterapi.users_table.close()
        theaterapi.tickets_table.close()
        os.remove('test_11.json')
        os.remove('test_12.json')

    def test_addtickets(self):
        theaterapi.tickets_table = tinydb.TinyDB('test_9.json')
        theaterapi.users_table = tinydb.TinyDB('test_10.json')
        client = theaterapi.app.test_client()
        self.assertEqual(client.post('/theaterapi/addticket', json={}).json['error'], 400)
        self.assertEqual(client.post('/theaterapi/addticket', json={
            'row': [],
            'place': [],
            'price': []
        }).json['error'], 400)
        self.assertEqual(client.post('/theaterapi/addticket').json['error'], 400)
        theaterapi.users_table.insert({
            'username': 'test_user',
            'password': argon2.hash('test_password'),
            'id': '123',
            'admin': True
        })
        resp_login = client.post('/theaterapi/login', json={
            'username': 'test_user',
            'password': 'test_password'
        })
        self.assertEqual(resp_login.status_code, 200)
        jwt_token = resp_login.json['jwt']
        resp_tickets = client.post('/theaterapi/addticket', json={
            'jwt': jwt_token,
            'row': 3,
            'place': 2,
            'price': 75
        })
        self.assertEqual(resp_tickets.status_code, 200)
        theaterapi.tickets_table.close()
        theaterapi.users_table.close()
        os.remove('test_9.json')
        os.remove('test_10.json')

    def test_scheme(self):
        client = theaterapi.app.test_client()
        theaterapi.server_config = {
            'scheme': [6, 3]
        }
        self.assertEqual(client.get('/theaterapi/scheme').json, [6, 3])


if __name__ == '__main__':
    unittest.main()
