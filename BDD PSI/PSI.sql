CREATE DATABASE LivinParis;
USE LivinParis;

CREATE TABLE utilisateur (
    id_utilisateur VARCHAR(50) PRIMARY KEY,
    nom VARCHAR(50),
    prenom VARCHAR(50),
    adresse VARCHAR(255),
    telephone VARCHAR(20),
    email VARCHAR(100) ,
    mot_de_passe VARCHAR(255)
);

CREATE TABLE client (
    id_client VARCHAR(50) PRIMARY KEY,
    FOREIGN KEY (id_client) REFERENCES utilisateur(id_utilisateur) ON DELETE CASCADE
);

CREATE TABLE particulier (
    id_particulier VARCHAR(50) PRIMARY KEY,
    FOREIGN KEY (id_particulier) REFERENCES client(id_client) ON DELETE CASCADE
);

CREATE TABLE entreprise_locale (
    id_entreprise VARCHAR(50) PRIMARY KEY,
    nom_entreprise VARCHAR(100),
    referent VARCHAR(100),
    FOREIGN KEY (id_entreprise) REFERENCES client(id_client) ON DELETE CASCADE
);

CREATE TABLE cuisinier (
    id_cuisinier VARCHAR(50) PRIMARY KEY,
    FOREIGN KEY (id_cuisinier) REFERENCES utilisateur(id_utilisateur) ON DELETE CASCADE
);

CREATE TABLE plat (
    id_plat VARCHAR(50) PRIMARY KEY,
    nom VARCHAR(100),
    type VARCHAR(50), -- Entrée, Plat principal, Dessert
    date_fabrication DATE,
    date_peremption DATE,
    nb_personnes INT,
    prix DECIMAL(10,2),
    nationalite VARCHAR(50),
    regime VARCHAR(50),
    ingredients TEXT,
    photo VARCHAR(255),
    id_cuisinier VARCHAR(50),
    FOREIGN KEY (id_cuisinier) REFERENCES cuisinier(id_cuisinier) ON DELETE CASCADE,
    FOREIGN KEY (id_commande) REFERENCES commande(id_commande) ON DELETE CASCADE
);

CREATE TABLE commande (
    id_commande VARCHAR(50) PRIMARY KEY,
    date_commande DATE,
    montant_total DECIMAL(10,2),
    id_client VARCHAR(50),
    FOREIGN KEY (id_client) REFERENCES client(id_client) ON DELETE CASCADE
);

CREATE TABLE ligne_commande (
    id_commande_ligne VARCHAR(50) PRIMARY KEY,
    id_commande VARCHAR(50),
    id_plat VARCHAR(50),
    quantite INT,
    date_livraison DATE,
    lieu_livraison VARCHAR(255),
    FOREIGN KEY (id_commande) REFERENCES commande(id_commande) ON DELETE CASCADE,
    FOREIGN KEY (id_livraison) REFERENCES livraison(id_livraison) ON DELETE CASCADE
);

CREATE TABLE livraison (
    id_livraison VARCHAR(50) PRIMARY KEY,
    date_livraison DATE,
    zone VARCHAR(50),
    id_cuisinier VARCHAR(50),
    FOREIGN KEY (id_cuisinier) REFERENCES cuisinier(id_cuisinier) ON DELETE CASCADE
);

CREATE TABLE transaction_ (
    id_transaction VARCHAR(50) PRIMARY KEY,
    montant DECIMAL(10,2),
    statut_paiement VARCHAR(50),
    id_commande VARCHAR(50),
    FOREIGN KEY (id_commande) REFERENCES commande(id_commande) ON DELETE CASCADE
);
