import os
import unittest

import jwt
import tinydb

import theaterapi


class TheaterAPITest(unittest.TestCase):
    # Placeinfo тесты

    def test_placeinfo_invalid(self):
        client = theaterapi.app.test_client()
        self.assertEqual(client.post('/theaterapi/placeinfo').json['error'], 400)

    def test_placeinfo_invalid_values(self):
        client = theaterapi.app.test_client()
        self.assertEqual(client.post('/theaterapi/placeinfo', json={
            'row': [],
            'place': {}
        }).json['error'], 400)

    def test_placeinfo_valid_usecase(self):
        theaterapi.tickets_table = tinydb.TinyDB('test.json')
        client = theaterapi.app.test_client()
        theaterapi.tickets_table.insert({'row': 3, 'place': 4, 'free': True, 'id': ''})
        result = client.post('/theaterapi/placeinfo', json={
            'row': 3,
            'place': 4
        }).json
        self.assertEqual(result['row'], 3)
        self.assertEqual(result['place'], 4)
        self.assertEqual(result['free'], True)
        self.assertEqual(result['id'], '')
        theaterapi.tickets_table.close()
        os.remove('test.json')

    # Register тесты

    def test_register_invalid(self):
        theaterapi.tickets_table = tinydb.TinyDB('test.json')
        client = theaterapi.app.test_client()
        self.assertEqual(client.post('/theaterapi/register').json['error'], 400)
        theaterapi.tickets_table.close()
        os.remove('test.json')

    def test_register_invalid_values(self):
        theaterapi.tickets_table = tinydb.TinyDB('test.json')
        client = theaterapi.app.test_client()
        self.assertEqual(client.post('/theaterapi/register', json={
            'username': [],
            'password': {}
        }).json['error'], 400)
        theaterapi.tickets_table.close()
        os.remove('test.json')

    def test_register_valid_usecase(self):
        theaterapi.users_table = tinydb.TinyDB('test.json')
        client = theaterapi.app.test_client()
        client.post('/theaterapi/register', json={
            'username': 'test_user',
            'password': 'test_password'
        })
        q = tinydb.Query()
        self.assertEqual(1, len(theaterapi.users_table.search(
            q.username == 'test_user'
        )))
        theaterapi.users_table.close()
        os.remove('test.json')

    # Login тесты

    def test_login_invalid(self):
        client = theaterapi.app.test_client()
        client.post('/theaterapi/register', json={})
        self.assertEqual(client.post('/theaterapi/login').json['error'], 400)

    def test_login_invalid_values(self):
        client = theaterapi.app.test_client()
        self.assertEqual(client.post('/theaterapi/register', json={
            'username': [],
            'password': {}
        }).json['error'], 400)

    def test_login_valid_usecase(self):
        theaterapi.users_table = tinydb.TinyDB('test.json')
        client = theaterapi.app.test_client()
        client.post('/theaterapi/register', json={
            'username': 'test_user',
            'password': 'test_password'
        })
        q = tinydb.Query()
        self.assertEqual(1, len(theaterapi.users_table.search(
            q.username == 'test_user'
        )))
        theaterapi.users_table.close()
        os.remove('test.json')


if __name__ == '__main__':
    unittest.main()
