from django.contrib import admin
from TbspRpgAdmin.admin import MultiDBModelAdmin
from .models import Adventure
from .models import Game
from .models import EventTypePosition
from .models import ProcessedEvent
from .models import EfMigrationHistory

# Register your models here.
class MultiDBModelAdminGameApi(MultiDBModelAdmin):
    using = 'gameapi'

admin.site.register(Adventure, MultiDBModelAdminGameApi)
admin.site.register(Game, MultiDBModelAdminGameApi)
admin.site.register(EventTypePosition, MultiDBModelAdminGameApi)
admin.site.register(ProcessedEvent, MultiDBModelAdminGameApi)
admin.site.register(EfMigrationHistory, MultiDBModelAdminGameApi)
