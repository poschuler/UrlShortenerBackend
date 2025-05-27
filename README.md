# UrlShortenerBackend

## <ins>Supuestos</ins>

***

**Tamaño de la URL:** 

La URL debe de ser lo más corta posible

**Caracteres validos para la URL:**

Los caracteres válidos son

[0-9]  [a-z]  [A-Z] => 10 + 26 + 26 = 62

62 posibles caracteres

**Tráfico esperado:**

100 millones de URLs son generadas diariamente

=> Operaciones de escritura por segundo:

100,000,000 / 24 / 3600 => 1158

Asumiendo un ratio de lectura/escritura de 10:1

=> Operaciones de lectura por segundo:

1158 * 10 => 11,580

Asumiendo que debemos soportar 10 años de vida para la aplicación 

=> 100,000,000 * 365 * 10 = 365,000,000,000 URL's a grabar

Espacio necesario

Asumiendo que cada URL tiene un tamaño de 100

=> 365,000,000,000  * 100 bytes = 36,500,000,000,000 bytes => ~ 36.5 TB


**Función Hash**

longURL -> hash fn() -> shortCode

longURL -> debe ser hasheada a shortCode
shortCode -> debe ser único
shortCode -> shortCode debe permitir recuperar la longURL

Basicamente tendremos una tabla que relacion shortCode con longURL donde shortCode es unique

**Tamaño del hash**

Tenemos 62 posibles caracteres que se pueden usar para crear el shortCode y debemos satisfacer 
el requerimiento no funcional de poder soportar 365,000,000,000 registros.

Adicionalmente nos piden que la URL sea lo mas corta posible.

62^1 -> 62
62^2 -> 3,844
62^3 -> 238,328
62^4 -> 14,776,336
62^5 -> 916,132,832
62^6 -> 56,800,235,584
62^7 -> 3,521,614,606,208

Como vemos al utilizar 7 caracteres podemos cubrir el requerimiento.

Para hacer el hash, podemos utilizar algoritmos como MD5 SHA-1 o SHA-256

MD5 : 5d41402abc4b2a76b9719d911017c592
SHA-1 : 7f83b1657ff1fc539292dc181483e528f8004f2f
SHA-256 : a3f124c6ad1602983c27e8d53372138b343f11d1e45903268809c9527d2c3e1e

todos los algoritmos nos dan un código que excede los 7 dígitos que hemos estimado para el shortCode

podríamos tomar los primeros 7 caracteres del hash pero nos enfrentaríamos a colisiones y tendríamos que validar la existencia del código para concatenarle algún valor y regenerar el hash. Sería costoso a nivel de queries.


Otra forma de crear un shortCode es realizando una conversión a Base 62

Para convertir un número decimal a Base 62, utilizamos la división sucesiva por la base y mapeamos los restos a los caracteres del conjunto dado: 0-9, a-z, A-Z.

El conjunto de caracteres de Base 62:

0 a 9 (10 caracteres)
a a z (26 caracteres)
A a Z (26 caracteres)

La asignación de valores a los caracteres es la siguiente:

0 = 0
1 = 1
...
9 = 9
a = 10
b = 11
...
z = 35
A = 36
B = 37
...
Z = 61

Paso 1: Dividir 15678 entre 62.
15678 ÷ 62 = 252 con un resto de 54.
El resto 54 corresponde al carácter S.

Paso 2: Dividir el cociente 252 entre 62.
252 ÷ 62 = 4 con un resto de 4.
El resto 4 corresponde al carácter 4.

Paso 3: Dividir el cociente 4 por 62.
4 ÷ 62 = 0 con un resto de 4.
El resto 4 corresponde al carácter 4.

15678 ->  en Base 62 ->  44S

esto nos permite maneter un máximo de 7 caracteres como shortCode y tener códigos únicos. Pero introduce la necesidad de contar con un Generador de IDs únicos.



