DROP SCHEMA fleuriste;
CREATE SCHEMA fleuriste;
CREATE TABLE IF NOT EXISTS Fleurs(
	nom_fleurs VARCHAR(20),
    prix FLOAT,
    disponibilité VARCHAR(40),
    nb_achat INTEGER,
    PRIMARY KEY(nom_fleurs)
);
CREATE TABLE IF NOT EXISTS Magasin(
	idMagasin INT,
    ville VARCHAR(40),
	nom_proprio VARCHAR(40),
    PRIMARY KEY(idMagasin)
);

CREATE TABLE IF NOT EXISTS produit(
	idproduit INT,
	nom_produit VARCHAR(40),
    prix float,
    stock INT,
    idmagasin INT,
    FOREIGN KEY(idmagasin)REFERENCES Magasin(idMagasin),
    PRIMARY KEY(idproduit)
);

CREATE TABLE IF NOT EXISTS clients (
    id_client INTEGER,
    nom VARCHAR(40),
    prenom VARCHAR(40),
    num_tel INT,
    courriel VARCHAR(40),
    mdp INTEGER,
    adresse_facturation VARCHAR(40),
    carte_credit VARCHAR(20),
    statut_fidelite VARCHAR(20),
    PRIMARY KEY (id_client)
);
CREATE TABLE IF NOT EXISTS bouquet_standard (
	idbouquet INT,
    nom_bouquet VARCHAR(40),
	idmagasin INT,
    composition VARCHAR(80),
    prix_bouquet double,
    occasion VARCHAR(20),
    quantite INTEGER,
    PRIMARY KEY (idbouquet),
    FOREIGN KEY(idmagasin) REFERENCES magasin(idMagasin)
);
CREATE TABLE IF NOT EXISTS commande (
	magasin INT,
    id_commande INTEGER,
    id_client INTEGER,
    etat VARCHAR(6),
    adresse_livraison VARCHAR(40),
    date_livraison date,
    message VARCHAR(40),
    date_commande date,
    prix DOUBLE,
    idbouquet INT,
    PRIMARY KEY (id_commande),
    FOREIGN KEY (id_client) REFERENCES clients(id_client),
    FOREIGN KEY (magasin) REFERENCES Magasin(idMagasin),
	FOREIGN KEY (idbouquet) REFERENCES bouquet_standard(idbouquet)
);
CREATE TABLE IF NOT EXISTS bouquet_perso (
    idproduit INT,
    idcommande INT,
    nom_fleurs VARCHAR(20),
    arrangement VARCHAR(200),
    prix_max FLOAT,
    PRIMARY KEY(arrangement),
    FOREIGN KEY (nom_fleurs) REFERENCES fleurs(nom_fleurs),
    FOREIGN KEY (idproduit) REFERENCES produit(idproduit),
    FOREIGN KEY (idcommande) REFERENCES commande(id_commande)
);



#on ajoute les données magasin,produit,bouquet standard dans notre base de données
INSERT INTO Magasin VALUES(1,'Antony','Antoine Journu');
INSERT INTO Magasin VALUES(2,'Neuilly','Sophie Delacourt');
INSERT INTO Magasin VALUES(3,'Saint-Cloud','Martin Germain-Durand');
INSERT INTO Magasin VALUES(4,'Boulogne-Billancourt','Julie Delacourt');
INSERT INTO Magasin VALUES(5,'Paris','Henri Lebrun');
INSERT INTO produit VALUES(1,"Vase",30,5,1);
INSERT INTO produit VALUES(2,"Ruban",3.50,8,1);
INSERT INTO produit VALUES(7,"Boîte",6,9,1);
INSERT INTO produit VALUES(8,"Vase",6,35,2);
INSERT INTO produit VALUES(3,"Ruban",3.50,10,2);
INSERT INTO produit VALUES(4,"Boîte",6,9,2);
INSERT INTO produit VALUES(5,"Boîte",6,15,3);
INSERT INTO produit VALUES(6,"Vase",6,35,3);
INSERT INTO produit VALUES(9,"",6,35,3);
INSERT INTO produit VALUES(10,"Boîte",6,4,4);
INSERT INTO produit VALUES(11,"Vase",30,2,4);
INSERT INTO produit VALUES(12,"Ruban",3.5,15,4);
INSERT INTO produit VALUES(13,"Boîte",6,14,5);
INSERT INTO produit VALUES(14,"Vase",30,6,5);
INSERT INTO produit VALUES(15,"Ruban",3.5,0,5);

INSERT INTO Fleurs VALUES ('Gerbera',5.00,'à l''année',0);
INSERT INTO Fleurs VALUES ('Ginger',4.00,'à l''année',0);
INSERT INTO Fleurs VALUES ('Glaïeul',1.00,'mai à novembre',0);
INSERT INTO Fleurs VALUES ('Marguerite',2.25,'à l''année',0);
INSERT INTO Fleurs VALUES ('Rose rouge',2.50,'à l''année',0);
INSERT INTO bouquet_standard VALUES (1,'Gros Merci',1,'Arrangement floral avec marguerites et verdure',45,'Toute occasion',8);
INSERT INTO bouquet_standard VALUES (2,'L''amoureux',1,'Arrangement floral avec roses blanches et roses rouges',65,'St-Valentin',10);
INSERT INTO bouquet_standard VALUES (3,'L''exotique',1,'Arrangement floral avec ginger, oiseaux du paradis, roses et genet',40,'Toute occasion',5);
INSERT INTO bouquet_standard VALUES (4,'Maman',1,'Arrangement floral avec gerbera, roses blanches, lys et alstroméria',80,'Fête des mères',20);
INSERT INTO bouquet_standard VALUES (5,'Vive la mariée',1,'Arrangement floral avec lys et orchidées ',120,'Mariage',4);
INSERT INTO bouquet_standard VALUES (6,'Gros Merci',2,'Arrangement floral avec marguerites et verdure',45,'Toute occasion',8);
INSERT INTO bouquet_standard VALUES (7,'L''amoureux',2,'Arrangement floral avec roses blanches et roses rouges',65,'St-Valentin',10);
INSERT INTO bouquet_standard VALUES (8,'L''exotique',2,'Arrangement floral avec ginger, oiseaux du paradis, roses et genet',40,'Toute occasion',5);
INSERT INTO bouquet_standard VALUES (9,'Maman',2,'Arrangement floral avec gerbera, roses blanches, lys et alstroméria',80,'Fête des mères',20);
INSERT INTO bouquet_standard VALUES (10,'Vive la mariée',2,'Arrangement floral avec lys et orchidées ',120,'Mariage',4);
INSERT INTO bouquet_standard VALUES (11,'Gros Merci',3,'Arrangement floral avec marguerites et verdure',45,'Toute occasion',8);
INSERT INTO bouquet_standard VALUES (12,'L''amoureux',3,'Arrangement floral avec roses blanches et roses rouges',65,'St-Valentin',10);
INSERT INTO bouquet_standard VALUES (13,'L''exotique',3,'Arrangement floral avec ginger, oiseaux du paradis, roses et genet',40,'Toute occasion',5);
INSERT INTO bouquet_standard VALUES (14,'Maman',3,'Arrangement floral avec gerbera, roses blanches, lys et alstroméria',80,'Fête des mères',20);
INSERT INTO bouquet_standard VALUES (15,'Vive la mariée',3,'Arrangement floral avec lys et orchidées ',120,'Mariage',4);
INSERT INTO bouquet_standard VALUES (16,'Gros Merci',4,'Arrangement floral avec marguerites et verdure',45,'Toute occasion',3);
INSERT INTO bouquet_standard VALUES (17,'L''amoureux',4,'Arrangement floral avec roses blanches et roses rouges',65,'St-Valentin',6);
INSERT INTO bouquet_standard VALUES (18,'L''exotique',4,'Arrangement floral avec ginger, oiseaux du paradis, roses et genet',40,'Toute occasion',15);
INSERT INTO bouquet_standard VALUES (19,'Maman',4,'Arrangement floral avec gerbera, roses blanches, lys et alstroméria',80,'Fête des mères',2);
INSERT INTO bouquet_standard VALUES (20,'Vive la mariée',5,'Arrangement floral avec lys et orchidées ',120,'Mariage',7);
INSERT INTO bouquet_standard VALUES (21,'Gros Merci',5,'Arrangement floral avec marguerites et verdure',45,'Toute occasion',3);
INSERT INTO bouquet_standard VALUES (22,'L''amoureux',5,'Arrangement floral avec roses blanches et roses rouges',65,'St-Valentin',12);
INSERT INTO bouquet_standard VALUES (23,'L''exotique',5,'Arrangement floral avec ginger, oiseaux du paradis, roses et genet',40,'Toute occasion',6);
INSERT INTO bouquet_standard VALUES (24,'Maman',5,'Arrangement floral avec gerbera, roses blanches, lys et alstroméria',80,'Fête des mères',15);
INSERT INTO bouquet_standard VALUES (25,'Vive la mariée',5,'Arrangement floral avec lys et orchidées ',120,'Mariage',13);

#on ajoute des clients et des commandes pour rendre le programme plus intéressant
INSERT INTO clients VALUES (1,'Leandre','Louise',0761313171,'louise.fleuriot@edu.devinci.fr',441548,'12 rue des Peupliés',4456992415638520,'sans statut'); 
INSERT INTO clients VALUES (2,'Fleuriot','Clémence',0761313171,'clem.fleuriot@edu.devinci.fr',441548,'12 rue des Peupliés',4456992415638520,'sans statut'); 
INSERT INTO clients VALUES (3,'Dupin','Elinor',0745652149,'elinor.dup@edu.devinci.fr',652341,'8 rue Marchand',4575369215624860,'sans statut');
INSERT INTO clients VALUES (4,'Doens','Aurélie',0666594782,'aurelie.dodo@edu.devinci.fr',694812,'61 avenue de la Traite',4988921453105695,'sans statut');
INSERT INTO clients VALUES (5,'Gérard','Natalia',0664483669,'natis.gerardo@edu.devinci.fr',956248,'14 rue du Chorizo',4864352110026958,'sans statut');
INSERT INTO clients VALUES (6,'De Wilde','Chloé',0662453463,'titou.wildos@edu.devinci.fr',456231,'85 rue de Neuilly',4156982014789520,'sans statut');
INSERT INTO clients VALUES (7,'Fleury','Nathan',0695488138,'lemacfleury@edu.devinci.fr',456210,'2 rue du Burger',4975126354920146,'sans statut');
INSERT INTO clients VALUES (8,'Dugay','Valentin',0789532146,'valoche.dug@edu.devinci.fr',956214,'61 rue de la Berangère',4023658471694682,'sans statut');
INSERT INTO clients VALUES (9,'Poute','Charles',0783112666,'Charlo.poutou@edu.devinci.fr',564829,'81 rue du Noble',7589621435682016,'sans statut');
INSERT INTO clients VALUES (10,'Journu','Marine',0769411831,'mama.jaune@edu.devinci.fr',111111,'1 rue de la trainée',4561963210568743,'sans statut');
INSERT INTO clients VALUES (11,'Guillemot','Kentin',0603622580,'kentin.guigui@edu.devinci.fr',195634,'46 rue de Monaco',5462359861472569,'sans statut');
INSERT INTO clients VALUES (12,'Laroudie','Alexandre',0631734078,'alex.laroue@edu.devinci.fr',446258,'63 rue de la roue',4862653142987852,'sans statut');
INSERT INTO clients VALUES (13,'Briand','Pierre',0669584521,'pierrot.shine@edu.devinci.fr',469521,'63 avenue Mozart',4215248697514630,'sans statut');
INSERT INTO clients VALUES (14,'Betton','Thomas',0745623159,'thomas.betoneuse@edu.devinci.fr',852046,'74 chemin du Faubourg',4401896357314286,'sans statut');
INSERT INTO clients VALUES (15,'Bouchet','Louis',0769587369,'labouche@edu.devinci.fr',394517,'52 route de la Reine',4156351486927635,'sans statut');

INSERT INTO commande VALUES(1,2,2,'CC','12 rue des Pins, Paris',20230428,'Tu nous manques',20230424,45,1);
INSERT INTO commande VALUES(1,3,3,'CC','45 avenue de Paris, Boulogne',20230429,'Gros Bisous',20230424,65,2);
INSERT INTO commande VALUES(1,4,4,'CC','12 rue des Pins, Paris',20230430,'Tu nous manques',20230424,40,3);
INSERT INTO commande VALUES(1,5,5,'CL','45 avenue de Paris, Boulogne',20230511,'Gros Bisous',20230424,80,4);
INSERT INTO commande VALUES(1,7,6,'CC','12 rue des Pins, Paris',20230512,'Tu nous manques',20230424,120,5);
INSERT INTO commande VALUES(1,8,8,'CC','45 avenue de Paris, Boulogne',20230429,'Gros Bisous',20230424,65,2);
INSERT INTO commande VALUES(1,9,9,'CC','12 rue des Pins, Paris',20230428,'Tu nous manques',20230424,45,1);
INSERT INTO commande VALUES(1,10,10,'CL','45 avenue de Paris,Boulogne',20230430,'Gros Bisous',20230424,40,3);
INSERT INTO commande VALUES(1,11,1,'CC','12 rue des Pins, Paris',20230501,'Tu nous manques',20230424,80,4);
INSERT INTO commande VALUES(1,12,2,'CC','45 avenue de Paris, Boulogne',20230529,'Gros Bisous',20230424,120,5);
INSERT INTO commande VALUES(1,13,3,'CL','12 rue des Pins, Paris',20230517,'Tu nous manques',20230424,120,5);
INSERT INTO commande VALUES(1,14,4,'CC','45 avenue de Paris, Boulogne',20230513,'Gros Bisous',20230424,80,4);

INSERT INTO commande VALUES(2,6,3,'CC','12 rue des Pins, Paris',20230528,'Tu nous manques',20230424,45,6);
INSERT INTO commande VALUES(2,15,14,'CC','12 rue des Pins, Paris',20230528,'Tu nous manques',20230424,45,6);
INSERT INTO commande VALUES(2,16,8,'CL','45 avenue de Paris, Boulogne',20230429,'Gros Bisous',20230424,65,7);
INSERT INTO commande VALUES(3,17,12,'CL','12 rue des Pins, Paris',20230530,'Tu nous manques',20230424,40,8);
INSERT INTO commande VALUES(4,18,12,'CC','45 avenue de Paris, Boulogne',20230429,'Gros Bisous',20230424,80,9);
INSERT INTO commande VALUES(3,19,11,'CC','12 rue des Pins, Paris',20230501,'Tu nous manques',20230424,120,10);
INSERT INTO commande VALUES(4,20,9,'CC','45 avenue de Paris, Boulogne',20230519,'Gros Bisous',20230424,65,7);
INSERT INTO commande VALUES(5,21,7,'CC','12 rue des Pins, Paris',20230518,'Tu nous manques',20230424,40,8);
INSERT INTO commande VALUES(5,22,4,'CL','45 avenue de Paris, Boulogne',20230504,'Gros Bisous',20230424,80,9);
INSERT INTO commande VALUES(5,23,2,'CC','12 rue des Pins, Paris',20230428,'Tu nous manques',20230424,120,10);
INSERT INTO commande VALUES(4,24,1,'CC','45 avenue de Paris, Boulogne',20230429,'Gros Bisous',20230424,120,10);
INSERT INTO commande VALUES(4,25,3,'CL','12 rue des Pins, Paris',20230504,'Tu nous manques',20230424,45,6);
INSERT INTO commande VALUES(2,26,3,'CC','45 avenue de Paris, Boulogne',20230517,'Gros Bisous',20230424,45,6);
INSERT INTO commande VALUES(1,1,3,'CC','12 rue des Pins, Paris',20230528,'Tu nous manques',20230424,45,6);
INSERT INTO commande VALUES(2,27,3,'CC','12 rue des Pins, Paris',20230528,'Tu nous manques',20230424,45,6);
INSERT INTO commande VALUES(5,28,5,'CL','45 avenue de Paris, Boulogne',20230429,'Gros Bisous',20230424,65,7);
INSERT INTO commande VALUES(5,29,5,'CL','12 rue des Pins, Paris',20230530,'Tu nous manques',20230424,40,8);
INSERT INTO commande VALUES(2,30,9,'CC','45 avenue de Paris, Boulogne',20230429,'Gros Bisous',20230424,80,9);
INSERT INTO commande VALUES(3,31,6,'CC','12 rue des Pins, Paris',20230501,'Tu nous manques',20230424,120,10);
INSERT INTO commande VALUES(4,32,9,'CC','45 avenue de Paris, Boulogne',20230519,'Gros Bisous',20230424,65,7);
INSERT INTO commande VALUES(2,33,7,'CC','12 rue des Pins, Paris',20230518,'Tu nous manques',20230424,40,8);
INSERT INTO commande VALUES(5,34,10,'CL','45 avenue de Paris, Boulogne',20230504,'Gros Bisous',20230424,80,9);
INSERT INTO commande VALUES(3,35,6,'CC','12 rue des Pins, Paris',20230428,'Tu nous manques',20230424,120,10);
INSERT INTO commande VALUES(4,36,7,'CC','45 avenue de Paris, Boulogne',20230429,'Gros Bisous',20230424,120,10);
INSERT INTO commande VALUES(4,37,3,'CL','12 rue des Pins, Paris',20230504,'Tu nous manques',20230424,45,6);
INSERT INTO commande VALUES(2,38,14,'CC','45 avenue de Paris, Boulogne',20230517,'Gros Bisous',20230424,45,6);




