import os

from passlib.hash import argon2

hash = argon2.hash(input('Пароль: '))
print('Ваш хеш: "{}"'.format(hash))
os.system('pause')