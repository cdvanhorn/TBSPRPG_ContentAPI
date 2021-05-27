from django.contrib import admin
from TbspRpgAdmin.admin import MultiDBModelAdmin
from .models import EventTypePosition
from .models import ProcessedEvent
from .models import EfMigrationHistory


# Register your models here.
class MultiDBModelAdminGameSystemApi(MultiDBModelAdmin):
    using = 'gamesystemapi'


admin.site.register(EventTypePosition, MultiDBModelAdminGameSystemApi)
admin.site.register(ProcessedEvent, MultiDBModelAdminGameSystemApi)
admin.site.register(EfMigrationHistory, MultiDBModelAdminGameSystemApi)
