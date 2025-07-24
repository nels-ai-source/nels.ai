import React, { useMemo, useEffect, useState } from 'react';
import { Container } from 'inversify';
import { useIntl } from 'umi';

import {
  VariableEngine,
  ScopeProvider,
  Scope,
  VariableContainerModule,
  type ASTNodeJSON,
} from '@flowgram.ai/variable-core';

interface StandaloneScopeProviderProps {
  children: React.ReactNode;
  initialData?: ASTNodeJSON;
  scopeId?: string;
  onScopeCreated?: (scope: Scope) => void;
}

const ERROR_STYLE = {
  background: '#fff3cd',
  border: '1px solid #ffeaa7',
  padding: '8px',
  margin: '8px 0',
  borderRadius: '4px',
  fontSize: '12px',
  color: '#856404'
};

function createStandaloneEngine(): VariableEngine | null {
  try {
    const container = new Container({
      defaultScope: 'Singleton',
      skipBaseClassChecks: true,
      autoBindInjectable: true
    });

    container.load(VariableContainerModule);

    if (!container.isBound(VariableEngine)) {
      console.warn('VariableEngine not bound in container, attempting manual binding');
      return null;
    }
    return container.get(VariableEngine);
  } catch (error) {
    console.error('Failed to create engine:', error);
    return null;
  }
}
export const StandaloneScopeProvider: React.FC<StandaloneScopeProviderProps> = ({
  children,
  initialData,
  scopeId = 'standalone-scope',
  onScopeCreated,
}) => {
  const intl = useIntl();
  const [engineError, setEngineError] = useState<string | null>(null);

  const variableEngine = useMemo(() => {
    try {
      const engine = createStandaloneEngine();
      if (!engine) {
        setEngineError(intl.formatMessage({ id: 'component.standaloneScope.error.missingDependencies' }));
        return null;
      }
      setEngineError(null);
      return engine;
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : intl.formatMessage({ id: 'component.standaloneScope.error.unknown' });
      setEngineError(intl.formatMessage({ id: 'component.standaloneScope.error.engineCreation' }, { message: errorMessage }));
      console.error('VariableEngine creation error:', error);
      return null;
    }
  }, [intl]);

  const standaloneScope = useMemo(() => {
    if (!variableEngine) {
      return null;
    }

    try {
      const scope = variableEngine.createScope(scopeId, {
        standalone: true,
        createdAt: new Date().toISOString(),
      });

      if (initialData) {
        scope.setVar(initialData);
      }

      if (onScopeCreated) {
        onScopeCreated(scope);
      }

      return scope;
    } catch (error) {
      console.error('Failed to create scope:', error);
      const errorMessage = error instanceof Error ? error.message : intl.formatMessage({ id: 'component.standaloneScope.error.unknown' });
      setEngineError(intl.formatMessage({ id: 'component.standaloneScope.error.scopeCreation' }, { message: errorMessage }));
      return null;
    }
  }, [variableEngine, scopeId, initialData, onScopeCreated, intl]);

  useEffect(() => {
    return () => {
      try {
        if (standaloneScope && typeof standaloneScope.dispose === 'function' && typeof variableEngine?.chain.getCovers === 'function') {
          standaloneScope.dispose();
        }
        if (variableEngine && typeof variableEngine.dispose === 'function' && typeof variableEngine.chain.getCovers === 'function') {
          variableEngine.dispose();
        }
      } catch (error) {
        console.error('Error during cleanup:', error);
      }
    };
  }, [standaloneScope, variableEngine]);

  if (engineError || !variableEngine || !standaloneScope) {
    console.warn('StandaloneScopeProvider fallback mode:', engineError);

    const fallbackScope = { scope: null };

    return (
      <div>
        {process.env.NODE_ENV === 'development' && engineError && (
          <div style={ERROR_STYLE}>
            {intl.formatMessage({ id: 'component.standaloneScope.warning' })}: {engineError}
          </div>
        )}
        <ScopeProvider value={fallbackScope}>{children}</ScopeProvider>
      </div>
    );
  }

  return <ScopeProvider value={{ scope: standaloneScope }}>{children}</ScopeProvider>;
};