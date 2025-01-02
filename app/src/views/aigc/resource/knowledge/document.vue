<template>
    <el-container>
        <el-header>
            <div class="left-panel">
                <el-button type="primary" icon="el-icon-plus" @click="handleAddDocument" text>{{ $t("knowledgeDocument.btnAdd") }}</el-button>
            </div>
            <div class="right-panel">
                <div class="right-panel-search">
                    <el-input v-model="search.keyword" :placeholder="$t('knowledgeDocument.namePlaceholder')" clearable></el-input>
                    <el-button type="primary" icon="el-icon-search" @click="getList"></el-button>
                </div>
            </div>
        </el-header>
        <el-main v-loading="isLoading">
            <scTable ref="table" :apiObj="apiObj" @selection-change="selectionChange" stripe remoteSort remoteFilter>
                <el-table-column type="selection" width="50"></el-table-column>
                <el-table-column :label="$t('knowledgeDocument.name')">
                    {{ scope.row.name }}
                </el-table-column>
                <el-table-column :label="$t('knowledgeDocument.documentType')" width="200">
                    <template #default="scope">
                        <el-tag>{{ scope.row.documentType }}</el-tag>
                    </template>
                </el-table-column>
                <el-table-column :label="$t('knowledgeDocument.length')" width="200">
                    {{ scope.row.length/1024 }}k
                </el-table-column>
                <el-table-column :label="$t('knowledgeDocument.retrievalCount')" width="200">
                    {{ scope.row.retrievalCount }}
                </el-table-column>
                <el-table-column :label="$t('knowledgeDocument.paragraphCount')" width="200">
                    {{ scope.row.paragraphCount }}
                </el-table-column>
                <el-table-column :label="$t('knowledgeDocument.isEnabled')" width="200">
                    <template #default="scope">
                        <el-tag>{{ scope.row.isEnabled }}</el-tag>
                    </template>
                </el-table-column>
            </scTable>
        </el-main>
    </el-container>
    <add-document-dialog v-if="dialog.addDocument" ref="addDocumentDialog" @success="handleSaveSuccess" @closed="dialog.save=false"></add-document-dialog>

</template>
<script>
import addDocumentDialog from './addDocument';

export default {
    name: 'document',
    components: {
        addDocumentDialog,
    },
    data() {
        return {
            isLoading: false,
            dialog: {
                addDocument: false,
            },
            list: [],
            search: {
                maxResultCount: 16,
                skipCount: 0,
                sorting: 'Id',
            },
            apiObj: this.$API.aigc.knowledgeDocument.list,
        };
    },
    async mounted() {
        //  await this.getList();
    },
    methods: {
        async getList() {
            this.isLoading = true;
            try {
                var res = await this.$API.aigc.knowledgeDocument.list.post(
                    this.search
                );
                this.list = res.items;
            } finally {
                this.isLoading = false;
            }
        },
        handleAddDocument(){
            this.dialog.addDocument = true;
            this.$nextTick(() => {
                this.$refs.addDocumentDialog.open();
            });
        }
    },
};
</script>