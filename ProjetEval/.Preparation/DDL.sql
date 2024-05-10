-- CREATE DATABASE projettest_aspnetcore_cine;

-- \c projettest_aspnetcore_cine;

DROP TABLE utilisateur;
DROP TABLE profil;
DROP TABLE genre;

CREATE TABLE genre(
    id SERIAL PRIMARY KEY,
    nom VARCHAR(100) DEFAULT 'Genre non initialiser'
);


CREATE TABLE profil(
    id SERIAL PRIMARY KEY,
    nom VARCHAR(200) NOT NULL,
    prenom VARCHAR(300) NOT NULL,
    naissance TIMESTAMP NOT NULL,
    idGenre INTEGER REFERENCES genre(id),
    max DECIMAL NOT NULL,
    min DECIMAL NOT NULL,
    CONSTRAINT checkMaxMin CHECK (max > min)
);


CREATE TABLE utilisateur(
    id INTEGER PRIMARY KEY,
    adress VARCHAR(500),
    mdp VARCHAR(100) NOT NULL,
    idProfil INTEGER REFERENCES profil(id),
    recuperation VARCHAR(500),
    CONSTRAINT  adress CHECK (adress ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$'),
    CONSTRAINT  recuperation CHECK (recuperation ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$')
);

