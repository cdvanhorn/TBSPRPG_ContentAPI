# This is an auto-generated Django model module.
# You'll have to do the following manually to clean this up:
#   * Rearrange models' order
#   * Make sure each model has one field with primary_key=True
#   * Make sure each ForeignKey and OneToOneField has `on_delete` set to the desired behavior
#   * Remove `managed = False` lines if you wish to allow Django to create, modify, and delete the table
# Feel free to rename the models, but don't rename db_table values or field names.
from django.db import models


class EfMigrationHistoryContentApi(models.Model):
    migrationid = models.CharField(db_column='MigrationId', primary_key=True, max_length=150)  # Field name made lowercase.
    productversion = models.CharField(db_column='ProductVersion', max_length=32)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = '__EFMigrationsHistory'


class ConditionalSource(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    contentkey = models.UUIDField(db_column='ContentKey')  # Field name made lowercase.
    javascript = models.TextField(db_column='JavaScript', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'conditional_sources'


class Content(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    gameid = models.ForeignKey('GameContentApi', models.DO_NOTHING, db_column='GameId')  # Field name made lowercase.
    position = models.DecimalField(db_column='Position', max_digits=20, decimal_places=0)  # Field name made lowercase.
    text = models.TextField(db_column='Text', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'contents'


class EventTypePositionContentApi(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    eventtypeid = models.UUIDField(db_column='EventTypeId')  # Field name made lowercase.
    position = models.DecimalField(db_column='Position', max_digits=20, decimal_places=0)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'event_type_positions'


class GameContentApi(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'games'


class ProcessedEventContentApi(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    eventid = models.UUIDField(db_column='EventId')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'processed_events'


class SourceEn(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    contentkey = models.UUIDField(db_column='ContentKey')  # Field name made lowercase.
    text = models.TextField(db_column='Text', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'sources_en'


class SourceEsp(models.Model):
    id = models.UUIDField(db_column='Id', primary_key=True)  # Field name made lowercase.
    contentkey = models.UUIDField(db_column='ContentKey')  # Field name made lowercase.
    text = models.TextField(db_column='Text', blank=True, null=True)  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'sources_esp'