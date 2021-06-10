from django.db import models

class EfMigrationHistory(models.Model):
    migrationid = models.CharField(db_column='MigrationId', primary_key=True, max_length=150)  # Field name made lowercase.
    productversion = models.CharField(db_column='ProductVersion', max_length=32)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = '__EFMigrationsHistory'


class Adventure(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    name = models.TextField(db_column='Name', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'adventures'


class Location(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    initial = models.BooleanField(db_column='Initial')  # Field name made lowercase.
    adventureid = models.ForeignKey(Adventure, models.DO_NOTHING, db_column='AdventureId')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'locations'

class Route(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    locationid = models.ForeignKey(Location, models.DO_NOTHING, db_column='LocationId')  # Field name made lowercase.
    name = models.TextField(db_column='Name', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'routes'
