-- Liste des clients
SELECT * FROM client;

-- Liste des cuisiniers
SELECT * FROM cuisinier;

-- Liste des plats disponibles
SELECT nom, type, prix FROM plat;

-- Afficher les commandes avec leur montant total
SELECT id_commande, montant_total FROM commande;

-- Liste des commandes d’un client donné, ici  U2
SELECT * FROM commande WHERE id_client = 'U2';

-- Liste des utilisateurs triée par nom dans l'ordre croissant
SELECT * FROM utilisateur
ORDER BY nom;

-- Liste des livraisons prévues pour un cuisinier, ici  U1
SELECT * FROM livraison WHERE id_cuisinier = 'U1';

-- Liste des transactions avec leur statut
SELECT * FROM transaction_;

-- Liste des livraisons triée par date de livraison dans l'ordre décroissant
SELECT * FROM livraison
ORDER BY date_livraison DESC;


-- Afficher les plats dont la date de péremption est dépassée
SELECT * FROM plat WHERE date_peremption < CURDATE();

-- Nombre total de plats disponibles
SELECT COUNT(*) nombre_de_plats FROM plat;