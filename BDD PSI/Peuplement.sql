-- Ajout d'utilisateurs
INSERT INTO utilisateur (id_utilisateur, nom, prenom, adresse, telephone, email, mot_de_passe)
VALUES 
('U1', 'Leclerc', 'Basile', '12 rue de Rivoli, 75001', '0678543210', 'basile.leclerc@email.com', 'philou7'),
('U2', 'Garnier', 'Lina', '34 avenue Montaigne, 75008', '0765124789', 'lina.garnier@email.com', 'bfqdskjqf43z'),
('U3', 'Dumont', 'Noé', '5 boulevard Haussmann, 75009', '0612349875', 'noe.dumont@email.com', 'jaca43di'),
('U4', 'Perrot', 'Salomé', '78 rue Lafayette, 75010', '0754986352', 'salome.perrot@email.com', 'adit34'),
('U5', 'Bouvier', 'Elias', '42 quai de Seine, 75019', '0687654321', 'elias.bouvier@email.com', 'afoufou'),
('U6', 'Marchal', 'Inès', '99 rue de la Paix, 75002', '0798541236', 'ines.marchal@email.com', 'tiplouf'),
('U7', 'Lemoine', 'Esteban', '18 rue Oberkampf, 75011', '0721456983', 'esteban.lemoine@email.com', 'pikachu'),
('U8', 'Reynaud', 'Maëlys', '53 avenue des Ternes, 75017', '0678954123', 'maelys.reynaud@email.com', 'saltamenco6549'),
('U9', 'Foucher', 'Axel', '21 rue du Faubourg, 75020', '0698542136', 'axel.foucher@email.com', 'gouzougouzou12'),
('U10', 'Charpentier', 'Lilia', '7 place de la Bastille, 75004', '0712365478', 'lilia.charpentier@email.com', 'nasdas44');

-- Ajout de clients
INSERT INTO client (id_client) VALUES ('U2'), ('U4'), ('U6'), ('U8'), ('U10');

-- Ajout de cuisiniers (certains utilisateurs sont à la fois clients et cuisiniers)
INSERT INTO cuisinier (id_cuisinier) VALUES ('U1'), ('U3'), ('U5'), ('U7'), ('U9');

-- Ajout de particuliers
INSERT INTO particulier (id_particulier) VALUES ('U2'), ('U4'), ('U6');

-- Ajout d'entreprises locales
INSERT INTO entreprise_locale (id_entreprise, nom_entreprise, referent)
VALUES 
('U8', 'Bistro Parisien', 'Maëlys Reynaud'),
('U10', 'Le Délice Urbain', 'Lilia Charpentier');

-- Ajout de plats
INSERT INTO plat (id_plat, nom, type, date_fabrication, date_peremption, nb_personnes, prix, nationalite, regime, ingredients, photo, id_cuisinier)
VALUES 
('P1', 'Bœuf Bourguignon', 'Plat principal', '2025-02-15', '2025-02-18', 4, 14.90, 'Française', 'Classique', 'Bœuf, Vin rouge, Carottes, Pommes de terre', 'boeuf_bourguignon.jpg', 'U1'),
('P2', 'Sushis Variés', 'Plat principal', '2025-02-14', '2025-02-16', 2, 22.50, 'Japonaise', 'Sans gluten', 'Saumon, Riz, Algues, Wasabi', 'sushis.jpg', 'U3'),
('P3', 'Ratatouille Provençale', 'Plat principal', '2025-02-13', '2025-02-17', 3, 10.50, 'Française', 'Végétarien', 'Tomates, Courgettes, Aubergines, Poivrons', 'ratatouille.jpg', 'U5'),
('P4', 'Mousse au Chocolat', 'Dessert', '2025-02-12', '2025-02-19', 6, 8.90, 'Française', 'Classique', 'Chocolat noir, Œufs, Sucre', 'mousse_chocolat.jpg', 'U7'),
('P5', 'Pad Thaï au Poulet', 'Plat principal', '2025-02-11', '2025-02-15', 2, 12.00, 'Thaïlandaise', 'Classique', 'Nouilles, Poulet, Sauce soja, Cacahuètes', 'pad_thai.jpg', 'U9');

-- Ajout de commandes
INSERT INTO commande (id_commande, date_commande, montant_total, id_client)
VALUES 
('C1', '2025-02-15', 22.50, 'U2'),
('C2', '2025-02-14', 14.90, 'U4'),
('C3', '2025-02-16', 10.50, 'U6'),
('C4', '2025-02-13', 8.90, 'U8'),
('C5', '2025-02-12', 12.00, 'U10');

-- Ajout de lignes de commande
INSERT INTO ligne_commande (id_commande_ligne, id_commande, id_plat, quantite, date_livraison, lieu_livraison)
VALUES 
('L1', 'C1', 'P2', 1, '2025-02-16', '34 avenue Montaigne, 75008'),
('L2', 'C2', 'P1', 1, '2025-02-15', '78 rue Lafayette, 75010'),
('L3', 'C3', 'P3', 1, '2025-02-17', '99 rue de la Paix, 75002'),
('L4', 'C4', 'P4', 1, '2025-02-18', '53 avenue des Ternes, 75017'),
('L5', 'C5', 'P5', 1, '2025-02-14', '7 place de la Bastille, 75004');

-- Ajout de livraisons
INSERT INTO livraison (id_livraison, date_livraison, zone, id_cuisinier)
VALUES 
('L1', '2025-02-16', 'Paris Centre', 'U3'),
('L2', '2025-02-15', 'Paris Est', 'U1'),
('L3', '2025-02-17', 'Paris Ouest', 'U5'),
('L4', '2025-02-18', 'Paris Nord', 'U7'),
('L5', '2025-02-14', 'Paris Sud', 'U9');

-- Ajout de transactions
INSERT INTO transaction_ (id_transaction, montant, statut_paiement, id_commande)
VALUES 
('T1', 22.50, 'Payé', 'C1'),
('T2', 14.90, 'En attente', 'C2'),
('T3', 10.50, 'Payé', 'C3'),
('T4', 8.90, 'Annulé', 'C4'),
('T5', 12.00, 'Payé', 'C5');