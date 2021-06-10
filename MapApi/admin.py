from django.contrib import admin
from TbspRpgAdmin.admin import MultiDBModelAdmin
from .models import Game
from .models import Location
from .models import EventTypePosition
from .models import ProcessedEvent
from .models import EfMigrationHistory
from .models import Route


# Register your models here.
class MultiDBModelAdminMapApi(MultiDBModelAdmin):
    using = 'mapapi'


admin.site.register(Game, MultiDBModelAdminMapApi)
admin.site.register(Location, MultiDBModelAdminMapApi)
admin.site.register(Route, MultiDBModelAdminMapApi)
admin.site.register(EventTypePosition, MultiDBModelAdminMapApi)
admin.site.register(ProcessedEvent, MultiDBModelAdminMapApi)
admin.site.register(EfMigrationHistory, MultiDBModelAdminMapApi)
