from django.contrib import admin
from TbspRpgAdmin.admin import MultiDBModelAdmin
from .models import Adventure
from .models import EfMigrationHistory
from .models import Location
from .models import Route

# Register your models here.
class MultiDBModelAdminAdventureApi(MultiDBModelAdmin):
    using = 'adventureapi'

class MultiDBModelAdminAdventureApiAdventure(MultiDBModelAdmin):
    using = 'adventureapi'

    list_display = ('id', 'name')

admin.site.register(Adventure, MultiDBModelAdminAdventureApiAdventure)
admin.site.register(Location, MultiDBModelAdminAdventureApi)
admin.site.register(Route, MultiDBModelAdminAdventureApi)
admin.site.register(EfMigrationHistory, MultiDBModelAdminAdventureApi)
