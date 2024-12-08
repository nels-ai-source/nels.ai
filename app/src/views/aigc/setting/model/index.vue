<template>
    <el-container>
        <el-aside width="300px" v-loading="showGrouploading">
            <el-container>
                <el-header>
                    <el-input :placeholder="$t('table.placeholderFilterText')" v-model="groupFilterText" clearable></el-input>
                </el-header>
                <el-main class="nopadding">
                    <el-tree ref="group" class="menu" node-key="id" :props="defaultProps" :data="group" :current-node-key="''" :highlight-current="true" :expand-on-click-node="false" :filter-node-method="groupFilterNode" @node-click="groupClick"></el-tree>
                </el-main>
            </el-container>
        </el-aside>
        <el-container>
            <el-header>
                <div class="left-panel">
                    <el-button type="primary" :disabled="selectionModel==null" icon="el-icon-plus" @click="add">{{ $t('model.addModelInstance') }}</el-button>
                    <el-button type="primary" plain :disabled="selection.length==0" @click="setKey">{{ $t('model.setKey') }}</el-button>
                </div>
                <div class="right-panel">
                    <div class="right-panel-search">
                        <el-input v-model="search.name" :placeholder="$t('table.placeholderFilterText')" clearable></el-input>
                        <el-button type="primary" icon="el-icon-search" @click="upsearch"></el-button>
                    </div>
                </div>
            </el-header>
            <el-main class="nopadding">
                <scTable ref="table" :apiObj="apiObj" @selection-change="selectionChange" stripe remoteSort remoteFilter>
                    <el-table-column type="selection" width="50"></el-table-column>
                    <el-table-column :label="$t('model.providerName')" width="200">
                        <template #default="scope">
                            <el-tag>{{ scope.row.providerName }}</el-tag>
                        </template>
                    </el-table-column>
                    <el-table-column :label="$t('model.nameAndDeploymentName')" prop="name" width="250">
                        <template #default="scope">
                            {{ scope.row.name }} {{ scope.row.deploymentName }}
                        </template>
                    </el-table-column>
                    <el-table-column :label="$t('model.typeName')" prop="typeName" width="200">
                        <template #default="scope">
                            <el-tag>{{ scope.row.typeName }}</el-tag>
                        </template>
                    </el-table-column>
                    <el-table-column :label="$t('model.isDefault')" prop="isDefault" width="120">
                        <template #default="scope">
                            <el-switch v-model="scope.row.isDefault" :disabled="scope.row.isDefault" @change="handleSetIsDefault(scope.row)" />
                        </template>
                    </el-table-column>
                    <el-table-column :label="$t('model.endpoint')" prop="endpoint"></el-table-column>
					<el-table-column :label="$t('model.description')" prop="description"></el-table-column>
                    <el-table-column :label="$t('table.handler')" fixed="right" align="right" width="160">
                        <template #default="scope">
                            <el-button-group>
                                <el-button text type="primary" size="small" @click="table_edit(scope.row, scope.$index)">{{$t('table.handlerEdit')}}</el-button>
                                <el-button text type="primary" size="small" @click="table_del(scope.row, scope.$index)">{{$t('table.handlerDelete')}}</el-button>
                            </el-button-group>
                        </template>
                    </el-table-column>

                </scTable>
            </el-main>
        </el-container>
    </el-container>
    <save-dialog v-if="dialog.save" ref="saveDialog" @success="upsearch" @closed="dialog.save=false"></save-dialog>、
    <set-key-dialog v-if="dialog.setKey" ref="setKeyDialog" @success="upsearch" @closed="dialog.setKey=false"></set-key-dialog>
</template>

<script>
import saveDialog from './save';
import setKeyDialog from './setKey';
export default {
    name: 'model',
    components: {
        saveDialog,
        setKeyDialog,
    },
    data() {
        return {
            dialog: {
                save: false,
                setKey: false,
            },
            showGrouploading: false,
            groupFilterText: '',
            group: [],
            apiObj: this.$API.aigc.modelInstance.list,
            selection: [],
            selectionModel: null,
            search: {
                name: null,
                modelId: null,
                modelProvider: null,
            },
            defaultProps: {
                children: 'children',
                label: 'name',
            },
        };
    },
    watch: {
        groupFilterText(val) {
            this.$refs.group.filter(val);
        },
    },
    mounted() {
        this.getGroup();
    },
    methods: {
        add() {
            let data = {
                modelId: this.selectionModel.id,
                name: this.selectionModel.name,
                endpoint: this.selectionModel.endpoint,
                properties: this.selectionModel.properties,
            };
            this.dialog.save = true;
            this.$nextTick(() => {
                this.$refs.saveDialog.open().setData(data);
            });
        },
        setKey() {
            let data = {
                ids: this.selection.map((item) => item.id),
                properties: this.selection.map((item) => item.properties),
            };
            this.dialog.setKey = true;
            this.$nextTick(() => {
                this.$refs.setKeyDialog.open().setData(data);
            });
        },
        table_edit(row) {
            this.dialog.save = true;
            this.$nextTick(() => {
                this.$refs.saveDialog.open('edit').setData(row);
            });
        },
        async table_del(row) {
            this.$confirm(
                this.$t('form.confirmDelete', { name: row.name }),
                this.$t('form.delete'),
                {
                    type: 'warning',
                    confirmButtonText: this.$t('form.delete'),
                    confirmButtonClass: 'el-button--danger',
                }
            )
                .then(async () => {
                    await this.$API.aigc.modelInstance.delete.post(row.id);
                    await this.upsearch();
                })
                .catch(() => {});
        },
        selectionChange(selection) {
            this.selection = selection;
        },
        async getGroup() {
            this.showGrouploading = true;
            var res = await this.$API.aigc.model.list.post();
            this.showGrouploading = false;
            var allNode = { id: '', name: this.$t('table.all') };
            res.items.unshift(allNode);
            this.group = res.items;
        },
        groupFilterNode(value, data) {
            if (!value) return true;
            return data.name.indexOf(value) !== -1;
        },
        groupClick(data) {
            if (data.id == '') {
                this.search.modelProvider = null;
                this.search.modelId = null;
            } else if (data.parentId == null) {
                this.search.modelProvider = data.provider;
                this.search.modelId = null;
            } else if (data.parentId != null) {
                this.search.modelProvider = null;
                this.search.modelId = data.id;
                this.selectionModel = data;
            }

            this.upsearch();
        },
        upsearch() {
            this.$refs.table.upData(this.search);
        },
        handleSetIsDefault(row) {
            let me = this;
            let params = {
                id: row.id,
            };
            this.$API.aigc.modelInstance.setIsDefault.post(params).then(() => {
                me.upsearch();
            });
        },
    },
};
</script>

<style>
</style>
