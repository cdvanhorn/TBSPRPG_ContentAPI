from django.contrib import admin
from TbspRpgAdmin.admin import MultiDBModelAdmin
from .models import Adventure
from .models import EfMigrationHistory
from .models import Location

# Register your models here.
class MultiDBModelAdminAdventureApi(MultiDBModelAdmin):
    using = 'adventureapi'

admin.site.register(Adventure, MultiDBModelAdminAdventureApi)
admin.site.register(Location, MultiDBModelAdminAdventureApi)
admin.site.register(EfMigrationHistory, MultiDBModelAdminAdventureApi)
