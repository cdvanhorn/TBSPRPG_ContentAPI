from django.contrib import admin
from TbspRpgAdmin.admin import MultiDBModelAdmin
from .models import Content
from .models import ConditionalSource
from .models import SourceEn
from .models import SourceEsp
from .models import EventTypePosition
from .models import ProcessedEvent
from .models import EfMigrationHistory
from .models import Game

# Register your models here.
class MultiDBModelAdminContentApi(MultiDBModelAdmin):
    using = 'contentapi'

class ContentApiSource(MultiDBModelAdminContentApi):
    list_display = ('contentkey', 'trim_text')

    def trim_text(self, obj):
        return obj.text[:100] + "..."


admin.site.register(Content, MultiDBModelAdminContentApi)
admin.site.register(ConditionalSource, MultiDBModelAdminContentApi)
admin.site.register(Game, MultiDBModelAdminContentApi)
admin.site.register(SourceEn, ContentApiSource)
admin.site.register(SourceEsp, ContentApiSource)
admin.site.register(EventTypePosition, MultiDBModelAdminContentApi)
admin.site.register(ProcessedEvent, MultiDBModelAdminContentApi)
admin.site.register(EfMigrationHistory, MultiDBModelAdminContentApi)
