# the puprosefor this file to have examples on how to create tables with specifics for test
# every block have to consist cteate and delete comand

# Test table with multiple primary key columns
CREATE table primkey3pc (
iccid varchar(20) NOT NULL,
imsi varchar(20) NOT NULL,
installed tinyint(1) NOT NULL DEFAULT 0,
purged tinyint(1) NOT NULL DEFAULT 0,
prop varchar(50) NOT NULL DEFAULT 'DEFAULT',
PRIMARY KEY (iccid, imsi)
);

DROP TABLE IF EXISTS primkey3pc;