<script setup>
import { ref } from 'vue';
import { VueFlow, useVueFlow, Position } from '@vue-flow/core';
import { Background } from '@vue-flow/background';
import { ControlButton, Controls } from '@vue-flow/controls';
import { MiniMap } from '@vue-flow/minimap';
import { initialEdges, initialNodes } from './initial-elements.js';
import Icon from './Icon.vue';

import StartNode from './components/StartNode.vue';
import KnowledgeNode from './components/KnowledgeNode.vue';
import LlmNode from './components/LlmNode.vue';

import useDragAndDrop from './useDragAndDrop';

const { onDragOver, onDrop, onDragLeave } = useDragAndDrop();
/**
 * `useVueFlow` provides:
 * 1. a set of methods to interact with the VueFlow instance (like `fitView`, `setViewport`, `addEdges`, etc)
 * 2. a set of event-hooks to listen to VueFlow events (like `onInit`, `onNodeDragStop`, `onConnect`, etc)
 * 3. the internal state of the VueFlow instance (like `nodes`, `edges`, `viewport`, etc)
 */
const {
    onInit,
    onNodeDragStop,
    onConnect,
    addEdges,
    setViewport,
    toObject,
    onNodeClick,
    onNodesChange,
    onEdgesChange,
    applyNodeChanges,
    applyEdgeChanges,
} = useVueFlow();

const targetPosition = Position.Top;
const sourcePosition = Position.Bottom;

const nodes = ref(initialNodes);

const edges = ref(initialEdges);

// our dark mode toggle flag
const dark = ref(false);

/**
 * This is a Vue Flow event-hook which can be listened to from anywhere you call the composable, instead of only on the main component
 * Any event that is available as `@event-name` on the VueFlow component is also available as `onEventName` on the composable and vice versa
 *
 * onInit is called when the VueFlow viewport is initialized
 */
onInit((vueFlowInstance) => {
    // instance is the same as the return of `useVueFlow`
    vueFlowInstance.fitView();
    setViewport({ x: 100, y: 100, zoom: 1 });
});

onNodeClick((event, node) => {
    console.log('Node Click', { event, node });
});

/**
 * onNodeDragStop is called when a node is done being dragged
 *
 * Node drag events provide you with:
 * 1. the event object
 * 2. the nodes array (if multiple nodes are dragged)
 * 3. the node that initiated the drag
 * 4. any intersections with other nodes
 */
onNodeDragStop(({ event, nodes, node }) => {
    console.log('Node Drag Stop', { event, nodes, node });
});

/**
 * onConnect is called when a new connection is created.
 *
 * You can add additional properties to your new edge (like a type or label) or block the creation altogether by not calling `addEdges`
 */
onConnect((connection) => {
    addEdges(connection);
});

onNodesChange(async (changes) => {
    const nextChanges = [];
    for (const change of changes) {
        console.log(change);
        if (change.type === 'remove') {
            if (change.id === '1') {
                //this.$message.success("开始节点不允许删除");
                return;
            } else {
                nextChanges.push(change);
            }
        } else {
            nextChanges.push(change);
        }
    }

    applyNodeChanges(nextChanges);
});
onEdgesChange(async (changes) => {
    applyEdgeChanges(changes);
});
/**
 * toObject transforms your current graph data to an easily persist-able object
 */
function logToObject() {
    console.log(toObject());
}

/**
 * Resets the current viewport transformation (zoom & pan)
 */
function resetTransform() {
    setViewport({ x: 0, y: 0, zoom: 1 });
}

function toggleDarkMode() {
    dark.value = !dark.value;
}
</script>

<template>
    <div class="dnd-flow" @drop="onDrop">
        <VueFlow :nodes="nodes" :edges="edges" :class="{ dark }" class="basic-flow" :default-zoom="1" :min-zoom="0.5" :max-zoom="4" @dragover="onDragOver" @dragleave="onDragLeave" delete-key-code="Delete" :apply-default="false">
            <Background pattern-color="#aaa" :gap="16" />
            <MiniMap />

            <Controls position="bottom-center" class="tools">
                <ControlButton title="Reset Transform" @click="resetTransform">
                    <Icon name="reset" />
                </ControlButton>

                <ControlButton title="Toggle Dark Mode" @click="toggleDarkMode">
                    <Icon v-if="dark" name="sun" />
                    <Icon v-else name="moon" />
                </ControlButton>

                <ControlButton title="Log `toObject`" @click="logToObject">
                    <Icon name="log" />
                </ControlButton>
            </Controls>

            <template #node-start="props">
                <StartNode v-bind="props" :source-position="sourcePosition" />
            </template>
            <template #node-knowledge="props">
                <KnowledgeNode v-bind="props" :source-position="sourcePosition" :target-position="targetPosition" />
            </template>
            <template #node-llm="props">
                <LlmNode v-bind="props" :source-position="sourcePosition" :target-position="targetPosition" />
            </template>
        </VueFlow>
    </div>
</template>
<style>
/* import the necessary styles for Vue Flow to work */
@import '@vue-flow/core/dist/style.css';

/* import the default theme, this is optional but generally recommended */
@import '@vue-flow/core/dist/theme-default.css';
@import '@vue-flow/controls/dist/style.css';

.vue-flow__node {
   /*  align-items: left;
    gap: 8px;
    padding: 8px 16px;
    border-radius: 8px;
    box-shadow: 0 0 10px #0003;
    background-color: #f3f4f6;*/
    padding: 10px;
    border-radius: 3px;
    width: 150px;
    font-size: 12px;
    text-align: center;
    border-width: 1px;
    border-style: solid;
    color: var(--vf-node-text);
    background-color: var(--vf-node-bg);
}
.vue-flow__node.selected {
    box-shadow: 0 0 0 2px #2563eb;
}

/*
.vue-flow__node:hover .vue-flow__handle {
    display: flex;
    content: '+';
    color: #fff;
    height: 1rem;
    width: 1rem;
    background-color: #2563eb;
    border-radius: 9999px;


}
.vue-flow__node:hover .vue-flow__handle::before {
    position: absolute;
    justify-content: center;
    align-items: center;
    height: 1rem;
    width: 1rem;
}
    */

.basic-flow .tools {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
}

.basic-flow .vue-flow__controls .note-controls {
    flex-wrap: wrap;
    justify-content: center;
}

.dnd-flow {
    flex-direction: column;
    display: flex;
    height: 100%;
}

</style>
