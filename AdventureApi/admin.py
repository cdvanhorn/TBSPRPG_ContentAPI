from django.contrib import admin
from TbspRpgAdmin.admin import MultiDBModelAdmin
from .models import Adventure
from .models import EfMigrationHistory
from .models import Location

# Register your models here.
class MultiDBModelAdminUserApi(MultiDBModelAdmin):
    using = 'adventureapi'

admin.site.register(Adventure, MultiDBModelAdminUserApi)
admin.site.register(Location, MultiDBModelAdminUserApi)
admin.site.register(EfMigrationHistory, MultiDBModelAdminUserApi)
