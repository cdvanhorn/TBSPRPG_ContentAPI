from django.db import models


class EfMigrationHistory(models.Model):
    migrationid = models.CharField(db_column='MigrationId', primary_key=True, max_length=150)  # Field name made lowercase.
    productversion = models.CharField(db_column='ProductVersion', max_length=32)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = '__EFMigrationsHistory'


class EventTypePosition(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    eventtypeid = models.UUIDField(db_column='EventTypeId')  # Field name made lowercase.
    position = models.DecimalField(db_column='Position', max_digits=20, decimal_places=0)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'event_type_positions'


class Game(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    adventureid = models.UUIDField(db_column='AdventureId')  # Field name made lowercase.
    userid = models.UUIDField(db_column='UserId')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'games'


class Location(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    gameid = models.OneToOneField(Game, models.DO_NOTHING, db_column='GameId')  # Field name made lowercase.
    locationid = models.UUIDField(db_column='LocationId')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'locations'


class ProcessedEvent(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    eventid = models.UUIDField(db_column='EventId')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'processed_events'

class Route(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    name = models.TextField(db_column='Name', blank=True, null=True)  # Field name made lowercase.
    locationid = models.ForeignKey(Location, models.DO_NOTHING, db_column='LocationId')  # Field name made lowercase.
    routeid = models.UUIDField(db_column='RouteId')  # Field name made lowercase.
    content = models.TextField(db_column='Content', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'routes'
