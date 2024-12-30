<template>
    <el-header>
        <div class="left-panel">
            <el-button type="primary" icon="el-icon-plus" @click="add" text>{{ $t("agent.btnAdd") }}</el-button>
        </div>
        <div class="right-panel">
            <div class="right-panel-search">
                <el-input v-model="search.keyword" :placeholder="$t('agent.namePlaceholder')" clearable></el-input>
                <el-button type="primary" icon="el-icon-search" @click="upsearch"></el-button>
            </div>
        </div>
    </el-header>
    <el-main v-loading="isLoading">
        <el-row>
            <el-col :xl=" 6" :lg="6" :md="8" :sm="12" :xs="24" v-for="item in list" :key="item.id" style="padding-right: 7.5px; padding-left: 7.5px;">
                <el-card class="agent agent-item">
                    <div class="pointer-cursor" @click="detail(item)">
                        <h2>{{item.name}}</h2>
                        <ul>
                            <li>
                                <h4><span class="description-clamp"> {{item.description}}</span></h4>
                            </li>
                            <li>
                                <p>{{ $t('agent.lastModificationTime') }}:{{item.lastModificationTime ||item.creationTime}}</p>
                            </li>
                        </ul>
                    </div>
                    <div class="bottom">
                        <div class="state">
                            <el-tag v-if="item.agentType==0" size="small">{{ $t('agent.agentType0') }}</el-tag>
                            <el-tag v-if="item.agentType==1" size="small">{{ $t('agent.agentType1') }}</el-tag>
                        </div>
                        <div class="handler">
                            <el-button type="primary" icon="el-icon-caret-right" @click="handleRun(item)" circle></el-button>
                            <el-button icon="el-icon-star" circle></el-button>

                            <el-dropdown trigger="click">
                                <el-button type="primary" icon="el-icon-more" circle plain></el-button>
                                <template #dropdown>
                                    <el-dropdown-menu>
                                        <el-dropdown-item @click="edit(item)">{{ $t('form.edit') }}</el-dropdown-item>
                                        <el-dropdown-item @click="del(item)" divided>{{ $t('form.delete') }}</el-dropdown-item>
                                    </el-dropdown-menu>
                                </template>
                            </el-dropdown>
                        </div>
                    </div>
                </el-card>
            </el-col>
        </el-row>
    </el-main>

    <save-dialog v-if="dialog.save" ref="saveDialog" @success="handleSaveSuccess" @closed="dialog.save=false"></save-dialog>

</template>

<script>
import saveDialog from './save';

export default {
    name: 'application',
    components: {
        saveDialog,
    },
    data() {
        return {
            isLoading: false,
            dialog: {
                save: false,
            },
            list: [],
            search: {
                maxResultCount: 16,
                skipCount: 0,
                sorting: 'Id',
            },
        };
    },
    async mounted() {
        await this.getList();
    },
    methods: {
        async getList() {
            this.isLoading = true;
            try {
                var res = await this.$API.aigc.agent.list.post(this.search);
                this.list = res.items;
            } finally {
                this.isLoading = false;
            }
        },
        add() {
            this.dialog.save = true;
            this.$nextTick(() => {
                this.$refs.saveDialog.open();
            });
        },
        edit(o) {
            this.dialog.save = true;
            this.$nextTick(() => {
                this.$refs.saveDialog.open('edit').setData(o);
            });
        },
        detail(o) {
            this.$router.push({
                path: '/aigc/agent/agent/detail',
                query: {
                    id: o.id,
                },
            });
        },
        async del(o) {
            this.$confirm(
                this.$t('form.confirmDelete', { name: o.name }),
                this.$t('form.delete'),
                {
                    type: 'warning',
                    confirmButtonText: this.$t('form.delete'),
                    confirmButtonClass: 'el-button--danger',
                }
            )
                .then(async () => {
                    await this.$API.aigc.agent.delete.post(o.id);
                    await this.getList();
                })
                .catch(() => {});
        },
        async handleSaveSuccess() {
            await this.getList();
        },
        async handleRun(o) {
            this.$router.push({
                path: '/aigc/agent/run',
                query: {
                    id: o.id,
                },
            });
        },
        async handleStar() {},
    },
};
</script>

<style scoped>
.pointer-cursor {
    cursor: pointer !important;
    height: 130px;
}
.agent {
    height: 210px;
}
.agent-item h2 {
    font-size: 15px;
    color: #3c4a54;
    padding-bottom: 15px;
    display: block;
    font-weight: bold;
    unicode-bidi: isolate;
}
.description-clamp {
    display: -webkit-box;
    -webkit-line-clamp: 3;
    line-clamp: 3;
    -webkit-box-orient: vertical;
    overflow: hidden;
    text-overflow: ellipsis;
}
.agent-item li {
    list-style-type: none;
    margin-bottom: 10px;
}
.agent-item li h4 {
    font-size: 12px;
    font-weight: normal;
    color: #999;
}
.agent-item li p {
    margin-top: 5px;
}
.agent-item .bottom {
    border-top: 1px solid #ebeef5;
    text-align: right;
    padding-top: 10px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.agent-add {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    text-align: center;
    cursor: pointer;
    color: #999;
}
.agent-add:hover {
    color: #409eff;
}
.agent-add i {
    font-size: 30px;
}
.agent-add p {
    font-size: 12px;
    margin-top: 20px;
}

.dark .agent-item .bottom {
    border-color: var(--el-border-color-light);
}
</style>
